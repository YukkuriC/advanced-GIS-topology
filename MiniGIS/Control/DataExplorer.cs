using MiniGIS.Data;
using MiniGIS.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniGIS.Control
{
    public class DataExplorer : MapSingle<DataExplorer>
    {
        object selector = null;

        Dictionary<Type, string> templateSimple, templateRich;

        public DataExplorer()
        {
            templateSimple = new Dictionary<Type, string>()
            {
                [typeof(GridLayer)] = "({0}, {1}) => {2}",
                [typeof(GeomLayer)] = "#{0} (C={1}, S={2})",
            };
            templateRich = new Dictionary<Type, string>()
            {
                [typeof(GridLayer)] = "行数: #{0} (X={3})\n列数: #{1} (X={4})\n交点取值: {2}",
                [typeof(GeomLayer)] = "ID: #{0}\n周长: {1}\n面积: {2}",
            };
        }

        public override void Render(ViewPort port, Graphics canvas)
        {
            if (selector == null) return;
            switch (selector)
            {
                case GeomPoly poly:
                    poly.Render(port, canvas, new Pen(Color.Yellow));
                    Pen arcPen = new Pen(Color.Red, 2);
                    foreach (var pair in poly.arcs)
                        pair.Item1.Render(port, canvas, arcPen);
                    if (poly.holes != null)
                        foreach (var poly1 in poly.holes)
                            foreach (var pair in poly1.arcs)
                                pair.Item1.Render(port, canvas, arcPen);
                    break;
            }
        }

        public override string DefaultText() => "使用鼠标移动/左键点击查看当前选定图层数据；点击鼠标右键移动屏幕；使用鼠标滚轮进行缩放";

        public override void Load()
        {
            MainForm.instance.labelInfo.Text = DefaultText();
        }

        public override void MouseMove(object sender, MouseEventArgs e) => MainForm.instance.labelInfo.Text = DisplayInfo(e, false);

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left: // 选定
                    var text = DisplayInfo(e, true);
                    if (text != null) MessageBox.Show(text, "图层信息");
                    break;
                case MouseButtons.Right:
                    MainForm.port.center = MainForm.port.WorldCoord(e.X, e.Y);
                    MainForm.port.Render(true);
                    break;
            }
        }

        public override void MouseWheel(object sender, MouseEventArgs e) => GeneralControl.WheelScale(e);

        string DisplayInfo(MouseEventArgs e, bool rich)
        {
            // 获取当前模板
            var template = rich ? templateRich : templateSimple;

            // 获取当前鼠标坐标与激活图层
            MainForm.port.WorldCoord(e.X, e.Y, out double worldX, out double worldY);
            Vector2 pos = new Vector2(worldX, worldY);
            Layer currLayer = MainForm.instance.layerView.SelectedNode as Layer;

            // 获取图层信息
            string res = rich ? null : DefaultText();
            switch (currLayer)
            {
                case GridLayer l1:
                case GeomLayer l2:
                    var info = LayerInfo(currLayer, pos);
                    if (info == null) break;
                    res = String.Format(template[currLayer.GetType()], info);
                    break;
            }

            return res;
        }

        #region viewers

        object[] LayerInfo(Layer rawLayer, Vector2 pos)
        {
            selector = null;
            switch (rawLayer)
            {
                // 栅格图层
                case GridLayer layer:
                    if (!layer.data.Include(pos)) return null;
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
                // 矢量图层
                case GeomLayer layer:
                    if (layer.polygons == null) break;
                    foreach (var poly in layer.polygons)
                    {
                        if (poly.Include(pos))
                        {
                            selector = poly;
                            return new object[]
                            {
                                poly.id,
                                poly.Circum, poly.Area,
                            };
                        }
                    }
                    break;
            }
            return null;
        }

        #endregion
    }
}
