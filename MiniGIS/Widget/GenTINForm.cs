using MiniGIS.Algorithm;
using MiniGIS.Data;
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
    public partial class GenTINForm : Form
    {
        public GenTINForm()
        {
            InitializeComponent();

            // 绑定图层
            var layers = Utils.BindLayers<GeomLayer>(comboLayer);

            // 初状态
            btnGen.Enabled = layers.Count > 0;
        }

        // 运行算法并创建图层
        private void GenTINLayer(object sender, EventArgs e)
        {
            GeomLayer layer = comboLayer.SelectedItem as GeomLayer;

            // 创建图层
            new TINLayer(layer.points, layer.Name + "_TIN").Add();

            // 结束
            Close();
        }

        // 启用按钮
        private void comboMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnGen.Enabled = true;
        }
    }
}
