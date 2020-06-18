using MiniGIS.Data;
using MiniGIS.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniGIS.Control
{
    public class DataExplorer : MapSingle<DataExplorer>
    {
        Dictionary<Type, string> templateSimple, templateRich;

        public DataExplorer()
        {
            templateSimple = new Dictionary<Type, string>()
            {
                [typeof(GridLayer)] = "({0}, {1}) => {2}",
            };
            templateRich = new Dictionary<Type, string>()
            {
                [typeof(GridLayer)] = "行数: #{0} (X={3})\n列数: #{1} (X={4})\n交点取值: {2}",
            };
        }

        public override string DefaultText() => "使用鼠标查看当前选定图层数据";

        public override void Load()
        {
            MainForm.instance.labelInfo.Text = DefaultText();
        }

        public override void MouseMove(object sender, MouseEventArgs e) => MainForm.instance.labelInfo.Text = DisplayInfo(e, false);

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            var text = DisplayInfo(e, true);
            if (text != null) MessageBox.Show(text, "图层信息");
        }

        string DisplayInfo(MouseEventArgs e, bool rich)
        {
            // 获取当前模板
            var template = rich ? templateRich : templateSimple;

            // 获取当前鼠标坐标与激活图层
            MainForm.port.WorldCoord(e.X, e.Y, out float worldX, out float worldY);
            Vector2 pos = new Vector2(worldX, worldY);
            Layer currLayer = MainForm.instance.layerView.SelectedNode as Layer;

            // 获取图层信息
            string res = rich ? null : DefaultText();
            switch (currLayer)
            {
                case GridLayer layer:
                    var info = LayerInfo(layer, pos);
                    if (info == null) break;
                    res = String.Format(template[layer.GetType()], info);
                    break;
            }

            return res;
        }

        #region viewers

        object[] LayerInfo(GridLayer layer, Vector2 pos)
        {
            if (!layer.data.Inside(pos)) return null;
            double xstep = (layer.data.XMax - layer.data.XMin) / (layer.data.XSplit);
            double ystep = (layer.data.YMax - layer.data.YMin) / (layer.data.YSplit);
            int i = (int)Math.Round((pos.X - layer.data.XMin) / xstep);
            int j = (int)Math.Round((pos.Y - layer.data.YMin) / ystep);
            double x = layer.data.XMin + xstep * i;
            double y = layer.data.YMin + ystep * j;
            return new object[]
            {
                i,j,
                layer.data[i,j],
                x,y
            };
        }

        #endregion
    }
}
