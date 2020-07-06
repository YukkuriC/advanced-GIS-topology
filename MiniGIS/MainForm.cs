using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MiniGIS.Layer;
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
            port = new ViewPort(rendererPort, layerView.Nodes.Cast<BaseLayer>());
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
            BindForm<GenTopoForm>(menuGenTopology);
            BindForm<ExportTopoForm>(menuExportTopology);
        }

        private void rendererPort_SizeChanged(object sender, EventArgs e)
        {
            port.Render();
        }

        private void MainForm_Load(object sender, EventArgs e) { }

        // 用于绑定弹出对话框按钮
        void BindForm<T>(ToolStripItem btn) where T : Form, new()
        {
            btn.Click += (object o, EventArgs e) => new T().ShowDialog();
        }

        private void UpdateSelection(object sender, TreeViewEventArgs e)
        {
            foreach (BaseLayer l in layerView.Nodes.OfType<BaseLayer>()) l.UpdateText();
        }
    }
}
