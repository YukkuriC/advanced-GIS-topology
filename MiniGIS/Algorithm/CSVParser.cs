using MiniGIS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Algorithm
{
    public static class CSVParser
    {
        #region parser

        static List<List<CSVNode>> table;
        static List<CSVNode> tableRow;
        static string currNode;
        static string errorMsg;
        static ParserStatus status;

        // 静态函数入口，解析CSV并检查行数
        public static List<List<CSVNode>> Parse(IEnumerable<char> raw)
        {
            // 读入数据
            ParseRaw(raw);

            // 验证每行等长
            for (int i = 1; i < table.Count; i++)
            {
                if (table[i].Count != table[i - 1].Count)
                {
                    errorMsg = String.Format("第{0}行不等长({1} != {2})", i + 1, table[i].Count, table[i - 1].Count);
                    throw new InvalidCastException(errorMsg);
                }
            }

            // 验证非空
            if (table.Count == 0 || table[0].Count == 0) throw new InvalidCastException("文件为空");

            // 返回结果
            return table;
        }

        // 读入文本转换为CSV
        static void ParseRaw(IEnumerable<char> raw)
        {
            // 初始化
            table = new List<List<CSVNode>>();
            tableRow = new List<CSVNode>();
            currNode = "";
            status = ParserStatus.RowStart;

            // 迭代字符
            int i = 0;
            foreach (char c in raw)
            {
                if (!ParseChar(c))
                {
                    throw new InvalidCastException(String.Format("字符#{0}: {1}", i, errorMsg));
                }
                i++;
            }

            // 是否读入最后一行
            switch (status)
            {
                case ParserStatus.InQuote:
                    throw new InvalidCastException("引号内容未结束");
                case ParserStatus.RowStart:
                    break;
                default:
                    AddRow();
                    break;
            }
        }

        // 读入单个字符
        static bool ParseChar(char c)
        {
            if (c == '\r' || c == '\0') return true;
            switch (status)
            {
                // 引号内：除双引号外均为文本内容
                case ParserStatus.InQuote:
                    if (c == '"') return TransStatus(ParserStatus.OutQuote);
                    currNode += c;
                    return true;
                // 起始处，根据起始字符转至相应状态
                case ParserStatus.RowStart:
                case ParserStatus.CellStart:
                    if (c == '"') return TransStatus(ParserStatus.InQuote);
                    return TransStatus(ParserStatus.NoQuote) && ParseChar(c);
                // 无引号文本，分隔符状态转移+双引号报错；其余同引号内
                case ParserStatus.NoQuote:
                    switch (c)
                    {
                        case ',':
                            return TransStatus(ParserStatus.CellStart);
                        case '\n':
                            return TransStatus(ParserStatus.RowStart);
                        case '"':
                            errorMsg = "引号前有内容";
                            return false;
                    }
                    goto case ParserStatus.InQuote;
                // 引号外：分隔符状态转移+双引号转义回到引号内；其余报错
                case ParserStatus.OutQuote:
                    switch (c)
                    {
                        case ',':
                            return TransStatus(ParserStatus.CellStart);
                        case '\n':
                            return TransStatus(ParserStatus.RowStart);
                        case '"':// 双引号转义
                            currNode += c;
                            return TransStatus(ParserStatus.InQuote);
                    }
                    errorMsg = "引号后有内容";
                    return false;
            }
            return true;
        }

        // 状态转移
        static bool TransStatus(ParserStatus newStatus)
        {
            switch (newStatus)
            {
                case ParserStatus.CellStart:
                    AddCell();
                    break;
                // 换行
                case ParserStatus.RowStart:
                    AddRow();
                    break;
            }
            status = newStatus;
            return true;
        }

        // 换单元
        static void AddCell()
        {
            tableRow.Add(new CSVNode(currNode));
            currNode = "";
        }

        // 换行
        static void AddRow()
        {
            AddCell();
            table.Add(tableRow);
            tableRow = new List<CSVNode>();
        }

        // 定义状态
        enum ParserStatus
        {
            RowStart,
            CellStart,
            NoQuote,
            InQuote,
            OutQuote,
        }

        #endregion

        // 导出CSV至点图层
        public static void OutputPoints(List<List<CSVNode>> source, List<GeomPoint> points, bool header, int colX, int colY, int colValue = -1, int colID = -1, int colName = -1)
        {
            // 创建图层
            for (int i = header ? 1 : 0; i < table.Count; i++)
            {
                List<CSVNode> line = table[i];
                double x = colX >= 0 ? (double)line[colX].valueNum : 0;
                double y = colY >= 0 ? (double)line[colY].valueNum : 0;
                GeomPoint pt = new GeomPoint(x, y, i);
                if (colID >= 0) pt.id = line[colID].valueInt;
                if (colName >= 0) pt.name = line[colName].valueString;
                if (colValue >= 0) pt.value = (double)line[colValue].valueNum;
                points.Add(pt);
            }
        }
    }

    public class CSVNode
    {
        public bool isNumber = false;
        public bool isInteger = false;
        public string valueString;
        public decimal valueNum;
        public int valueInt;

        public CSVNode(string val)
        {
            valueString = val;
            isNumber = Decimal.TryParse(val, out valueNum);
            isInteger = int.TryParse(val, out valueInt);
        }
    }
}
