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
            var layers = Utils.BindLayers<GridLayer>(comboLayer);

            // 初状态
            btnGen.Enabled = layers.Count > 0;
        }

        // 运行算法并创建图层
        private void GenGridLayer(object sender, EventArgs e)
        {
            GridLayer oldLayer = comboLayer.SelectedItem as GridLayer;
            if (oldLayer == null)
            {
                MessageBox.Show("没有可用的栅格图层", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            uint xstep = (uint)numericXStep.Value, ystep = (uint)numericYStep.Value;
            Grid newGrid = GenGrid.LinearInterpolation(oldLayer.data, xstep, ystep);
            new GridLayer(newGrid, oldLayer.Name + String.Format("_{0}x{1}加密", xstep, ystep)).Add();
            Close();
        }
    }
}
