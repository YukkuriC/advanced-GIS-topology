using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MiniGIS.Algorithm;
using MiniGIS.Data;
using MiniGIS.Layer;

namespace MiniGIS.Widget
{
    public partial class CSVLoader : Form
    {
        #region prop

        List<List<CSVNode>> table;
        bool[] colNum, colNumHeader;
        bool[] colInt, colIntHeader;

        ComboBox[] fields, strFields, intFields, numFields;

        #endregion

        #region method

        // 判断每列CSV是否可作为数字
        void ColumnCheck()
        {
            int ncol = table[0].Count;
            colNumHeader = new bool[ncol];
            colIntHeader = new bool[ncol];
            for (int c = 0; c < ncol; c++)
            {
                colNumHeader[c] = colIntHeader[c] = true;
            }
            for (int r = 1; r < table.Count; r++)
            {
                for (int c = 0; c < ncol; c++)
                {
                    colNumHeader[c] &= table[r][c].isNumber;
                    colIntHeader[c] &= table[r][c].isInteger;
                }
            }
            colNum = new bool[ncol];
            colInt = new bool[ncol];
            for (int c = 0; c < ncol; c++)
            {
                colNum[c] = colNumHeader[c] && table[0][c].isNumber;
                colInt[c] = colIntHeader[c] && table[0][c].isInteger;
            }

            // 更新选框内容
            UpdateFields();
        }

        // 读入CSV并更新UI
        private void OpenCSV(object sender = null, EventArgs e = null)
        {
            // 初始化控件
            panelSettings.Enabled = false;
            filePath.Text = "无";
            tableInfo.Text = "";

            // 读入CSV
            if (openCSV.ShowDialog() == DialogResult.OK)
            {
                // 读入
                char[] buffer;
                using (StreamReader reader = File.OpenText(openCSV.FileName))
                {
                    buffer = new char[reader.BaseStream.Length];
                    reader.Read(buffer, 0, (int)reader.BaseStream.Length);
                }

                // 加载CSV
                try
                {
                    table = CSVParser.Parse(buffer);
                    panelSettings.Enabled = true;
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "读取失败");
                }

                // 更新UI
                if (panelSettings.Enabled)
                {
                    ColumnCheck();
                    filePath.Text = Path.GetFileName(openCSV.FileName);
                    (sender as Button).Text = "更换";
                }
            }
        }

        // 更新下拉框内容
        private void UpdateFields(object sender = null, EventArgs e = null)
        {
            // 更新行列显示
            bool[] isNum, isInt;
            int ncol = table[0].Count, nrow = table.Count;
            if (readHeader.Checked)
            {
                isNum = colNumHeader;
                isInt = colIntHeader;
                nrow--;
            }
            else
            {
                isNum = colNum;
                isInt = colInt;
            }
            tableInfo.Text = String.Format("{0}行, {1}列", nrow, ncol);

            // 逐列加入内容
            List<CSVColOption> strSource = new List<CSVColOption>();
            strSource.Add(new CSVColOption(-1, "------", false));
            List<CSVColOption> numSource = strSource.ToList();
            List<CSVColOption> intSource = strSource.ToList();
            for (int i = 0; i < ncol; i++)
            {
                CSVColOption option = new CSVColOption(i, table[0][i].valueString, true);
                strSource.Add(option);
                if (isNum[i]) numSource.Add(option);
                if (isInt[i]) intSource.Add(option);
            }
            foreach (ComboBox field in strFields) field.DataSource = strSource.ToList();
            foreach (ComboBox field in intFields) field.DataSource = intSource.ToList();
            foreach (ComboBox field in numFields) field.DataSource = numSource.ToList();

            // 记录
            List<object> oldValues = new List<object>(from field in fields select field.SelectedValue);
        }


        // 导入CSV为多点对象
        private void ImportPoints(object sender = null, EventArgs e = null)
        {
            // 创建图层
            GeomLayer newLayer = new GeomLayer(GeomType.Point, filePath.Text);
            CSVParser.OutputPoints(
                table, newLayer.points, readHeader.Checked,
                (int)selectX.SelectedValue, (int)selectY.SelectedValue, (int)selectValue.SelectedValue,
                (int)selectID.SelectedValue, (int)selectName.SelectedValue
            );

            // 判断是否成功
            if (newLayer.points.Count > 0)
            {
                newLayer.Add(LayerTag.Points);
            }
            else
            {
                MessageBox.Show("图层为空", "插入失败");
            }

            // 关闭窗口
            Close();
        }

        #endregion

        public CSVLoader()
        {
            InitializeComponent();

            // 初始化域列表
            fields = new ComboBox[] { selectID, selectName, selectX, selectY, selectValue };
            strFields = new ComboBox[] { selectName };
            intFields = new ComboBox[] { selectID };
            numFields = new ComboBox[] { selectX, selectY, selectValue };

            // 绑定域数据源
            foreach (ComboBox field in fields)
            {
                field.DisplayMember = "Name";
                field.ValueMember = "ID";
            }
        }
    }

    class CSVColOption
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public CSVColOption(int id, string name, bool auto)
        {
            ID = id;
            if (auto)
            {
                Name = "#" + id.ToString();
                if (name.Length > 0) Name += ": " + name;
            }
            else Name = name;
        }
    }
}
