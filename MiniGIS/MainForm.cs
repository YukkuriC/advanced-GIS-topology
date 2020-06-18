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
            rendererPort.MouseEnter += controlManager.MouseIn;
            rendererPort.MouseLeave += controlManager.MouseOut;
            controlSet.SelectedIndexChanged += (object o, EventArgs e) => controlManager.Set(controlSet.SelectedIndex);
            controlSet.SelectedIndex = 0;

            // 绑定选框
            BindForm<CSVLoader>(menuLoadCSV);
            BindForm<GenGridForm>(menuGenGrid);
            BindForm<GridInterpolationForm>(menuGridInterpolation);
            BindForm<GenTINForm>(menuGenTIN);
            BindForm<GenContourForm>(menuGenContour);
            BindForm<ContourSmoothForm>(menuContourSmooth);
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
            // 测试TIN
            TINLayer newTIN = new TINLayer(newLayer.points, "TIN") { Visible = false };
            newTIN.Add();
            // 测试等值线
            API.ContourSmooth(
                (GeomLayer)API.Value2Contour(newTIN, Utils.Linear(-10, -20, 0, 27)).Add()
            ).Add();
        }

        // 用于绑定弹出对话框按钮
        void BindForm<T>(ToolStripItem btn) where T : Form, new()
        {
            btn.Click += (object o, EventArgs e) => new T().ShowDialog();
        }
    }
}
