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
    public partial class GridInterpolationForm : Form
    {
        #region methods

        #endregion

        public GridInterpolationForm()
        {
            InitializeComponent();

            // 绑定图层
            var layers = FormUtils.BindLayers<GridLayer>(comboLayer);

            // 初状态
            btnGen.Enabled = layers.Count > 0;
        }

        // 运行算法并创建图层
        private void GenGridLayer(object sender, EventArgs e)
        {
            API.GridInterpolation(comboLayer.SelectedItem as GridLayer, (uint)numericXStep.Value, (uint)numericYStep.Value).Add();
            Close();
        }
    }
}
