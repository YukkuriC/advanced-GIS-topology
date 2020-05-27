using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MiniGIS.Render;
using MiniGIS.Data;
using MiniGIS.Control;
using MiniGIS.Widget;
using MiniGIS.Algorithm;

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

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 测试数据
            char[] buffer;
            using (StreamReader reader = File.OpenText("../../../data.txt"))
            {
                buffer = new char[reader.BaseStream.Length];
                reader.Read(buffer, 0, (int)reader.BaseStream.Length);
            }
            var table = CSVParser.Parse(buffer);
            GeomLayer newLayer = new GeomLayer(GeomType.Point, "TEST");
            CSVParser.OutputPoints(table, newLayer.points, false, 2, 3, 4, 0, 1);
            newLayer.Add();
            newLayer.Focus(port);
        }

        private void menuLoadCSV_Click(object sender, EventArgs e)
        {
            new CSVLoader().ShowDialog();
        }
    }
}
