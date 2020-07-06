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
    public partial class GenTINForm : Form
    {
        public GenTINForm()
        {
            InitializeComponent();

            // 绑定图层
            var layers = FormUtils.BindLayers<GeomLayer>(comboLayer, LayerTag.Points);

            // 初状态
            btnGen.Enabled = layers.Count > 0;
        }

        // 运行算法并创建图层
        private void GenTINLayer(object sender, EventArgs e)
        {
            API.Point2TIN(comboLayer.SelectedItem as GeomLayer).Add(LayerTag.TIN);
            Close();
        }
    }
}
