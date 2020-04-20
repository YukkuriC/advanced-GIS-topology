using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MiniGIS.Render;
using MiniGIS.Data;
using MiniGIS.Control;

namespace MiniGIS
{
    public partial class MainForm : Form
    {
        public static MainForm instance;

        public static ViewPort port;
        public static ControlManager controlManager;
        public static Random random;

        public MainForm()
        {
            InitializeComponent();
            MainForm.instance = this;
            random = new Random();

            // 绑定事件
            port = new ViewPort(rendererPort, layerView.Nodes.Cast<Layer>());
            controlManager = new ControlManager();
            rendererPort.MouseDown += controlManager.MouseDown;
            rendererPort.MouseUp += controlManager.MouseUp;
            rendererPort.MouseMove += controlManager.MouseMove;
            rendererPort.MouseWheel += controlManager.MouseWheel;
        }

        private void rendererPort_SizeChanged(object sender, EventArgs e)
        {
            port.Render();
        }

        private void rendererPort_MouseHover(object sender, EventArgs e)
        {
            rendererPort.Focus();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 测试数据
            Random rnd = new Random();
            for (int i = 0; i < 5; i++)
            {
                GeomType type = GeomType.Polygon;
                GeomLayer tmp = new GeomLayer(
                    type,
                    String.Format("{0} #{1}", type, i)
                );
                for (int pc = 0; pc < 2; pc++)
                {
                    double cx = rnd.NextDouble() * 300 - 150,
                           cy = rnd.NextDouble() * 300 - 150,
                           r = rnd.NextDouble() * 60 + 60;
                    int ss = rnd.Next(5, 12);
                    // 点
                    List<GeomPoint> pts = new List<GeomPoint>();
                    for (int j = 0; j < ss; j++)
                    {
                        double r1 = r + rnd.NextDouble() * 30 - 15;
                        pts.Add(new GeomPoint(
                            cx + Math.Cos(Math.PI * j * 2 / ss) * r1,
                            cy + Math.Sin(Math.PI * j * 2 / ss) * r1,
                            pc * 1000 + j
                        ));
                    }
                    tmp.points.AddRange(pts);
                    pts.Add(pts[0]);
                    // 弧段
                    List<GeomArc> arcs = new List<GeomArc>();
                    int p1 = 0, p2;
                    while (p1 < pts.Count - 1)
                    {
                        p2 = rnd.Next(2, 6);
                        if (p1 + p2 > pts.Count - 2) p2 = pts.Count - p1;
                        arcs.Add(new GeomArc(pts.GetRange(p1, p2)));
                        p1 += p2 - 1;
                    }
                    tmp.arcs.AddRange(arcs);
                    // 多边形
                    tmp.polygons.Add(new GeomPoly(arcs));
                }
                tmp.Add();
            }
        }
    }
}
