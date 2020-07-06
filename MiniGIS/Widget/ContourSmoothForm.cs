using MiniGIS.Algorithm;
using MiniGIS.Data;
using MiniGIS.Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniGIS.Widget
{
    public partial class ContourSmoothForm : Form
    {
        public ContourSmoothForm()
        {
            InitializeComponent();

            // 绑定图层
            var layers = FormUtils.BindLayers<GeomLayer>(comboLayer, (from l in MainForm.instance.layerView.Nodes.OfType<GeomLayer>() where l.arcs != null select l));

            // 初状态
            btnGen.Enabled = layers.Count > 0;
        }

        private void GenSmooth(object sender, EventArgs e)
        {
            API.ContourSmooth(comboLayer.SelectedItem as GeomLayer).Add();
            Close();
        }
    }
}
