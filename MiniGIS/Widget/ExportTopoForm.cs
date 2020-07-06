using MiniGIS.Algorithm;
using MiniGIS.Render;
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
    public partial class ExportTopoForm : Form
    {
        public ExportTopoForm()
        {
            InitializeComponent();

            // 绑定图层
            var layers = FormUtils.BindLayers<GeomLayer>(comboLayer, (from l in MainForm.instance.layerView.Nodes.OfType<GeomLayer>() where l.topology != null select l));

            // 初状态
            btnGen.Enabled = layers.Count > 0;
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            if (API.ExportTopology(comboLayer.SelectedItem as GeomLayer))
                Close();
        }
    }
}
