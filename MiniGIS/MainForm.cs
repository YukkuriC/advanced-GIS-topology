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

        public MainForm()
        {
            InitializeComponent();
            MainForm.instance = this;

            // 绑定事件
            port = new ViewPort(rendererPort, layerView.Nodes.Cast<Layer>());
            controlManager = new ControlManager();
            rendererPort.MouseDown += controlManager.MouseDown;
            rendererPort.MouseUp += controlManager.MouseUp;
            rendererPort.MouseMove += controlManager.MouseMove;
            rendererPort.MouseWheel += controlManager.MouseWheel;

            // 测试数据
            GeomLayer tmp = new GeomLayer(GeomType.Polygon);
            tmp.points.Add(new GeomPoint(-10, 0));
            tmp.points.Add(new GeomPoint(0, 6));
            tmp.points.Add(new GeomPoint(5, 36));
            tmp.arcs.Add(new GeomArc(tmp.points));
            tmp.arcs[0].points.Add(tmp.points[0]);
            tmp.polygons.Add(new GeomPoly(tmp.arcs));
            tmp.Add();
        }

        private void rendererPort_SizeChanged(object sender, EventArgs e)
        {
            port.Render();
        }

        private void rendererPort_MouseHover(object sender, EventArgs e)
        {
            rendererPort.Focus();
        }
    }
}
