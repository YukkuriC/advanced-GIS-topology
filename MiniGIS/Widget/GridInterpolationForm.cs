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
    public partial class GridInterpolationForm : Form
    {
        #region methods

        #endregion

        public GridInterpolationForm()
        {
            InitializeComponent();

            // 绑定图层
            GridLayer tmp;
            List<GridLayer> layers = new List<GridLayer>();
            foreach (Layer l in MainForm.instance.layerView.Nodes)
            {
                tmp = l as GridLayer;
                if (tmp != null) layers.Add(tmp);
            }
            comboLayer.DataSource = layers;
            comboLayer.DisplayMember = "Text";
        }

        // 运行算法并创建图层
        private void GenGridLayer(object sender, EventArgs e)
        {
            GridLayer oldLayer = comboLayer.SelectedItem as GridLayer;
            uint xstep = (uint)numericXStep.Value, ystep = (uint)numericYStep.Value;
            Grid newGrid = GenGrid.LinearInterpolation(oldLayer.data, xstep, ystep);
            new GridLayer(newGrid, oldLayer.Name + String.Format("_{0}x{1}加密", xstep, ystep)).Add().Focus(MainForm.port);
            Close();
        }
    }
}
