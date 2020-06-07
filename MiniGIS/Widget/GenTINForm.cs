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
            GeomLayer tmp;
            List<GeomLayer> layers = new List<GeomLayer>();
            foreach (Layer l in MainForm.instance.layerView.Nodes)
            {
                tmp = l as GeomLayer;
                if (tmp != null && tmp.points != null && tmp.points.Count >= 3) layers.Add(tmp);
            }
            comboLayer.DataSource = layers;
            comboLayer.DisplayMember = "Text";
        }

        // 运行算法并创建图层
        private void GenTINLayer(object sender, EventArgs e)
        {
            GeomLayer layer = comboLayer.SelectedItem as GeomLayer;
            if (layer == null)
            {
                MessageBox.Show("没有可用的点图层", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            // 运行算法
            GeomLayer result = GenDelaunay.DelaunayScan(layer.points);

            // 创建图层
            result.Name = layer.Name + "_TIN";
            result.Add().Focus(MainForm.port);

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
