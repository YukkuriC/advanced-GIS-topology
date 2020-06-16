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
    public partial class GenGridForm : Form
    {
        #region methods

        // 验证输入合法性
        void ValidateBorders()
        {
            // 防止相等
            if (numericXMin.Value == numericXMax.Value)
            {
                numericXMin.Value -= (decimal)0.5;
                numericXMax.Value += (decimal)0.5;
            }
            if (numericYMin.Value == numericYMax.Value)
            {
                numericYMin.Value -= (decimal)0.5;
                numericYMax.Value += (decimal)0.5;
            }

            // 判断大小关系
            decimal tmp;
            if (numericXMin.Value > numericXMax.Value)
            {
                tmp = numericXMax.Value;
                numericXMax.Value = numericXMin.Value;
                numericXMin.Value = tmp;
            }
            if (numericYMin.Value > numericYMax.Value)
            {
                tmp = numericYMax.Value;
                numericYMax.Value = numericYMin.Value;
                numericYMin.Value = tmp;
            }
        }

        #endregion

        public GenGridForm()
        {
            InitializeComponent();

            // 绑定图层
            var layers = FormUtils.BindLayers<GeomLayer>(comboLayer);

            // 初状态
            comboMethod.SelectedIndex = 0;
            btnGen.Enabled = btnReset.Enabled = layers.Count > 0;
        }

        // 读取图层边界，设置默认值
        private void SetLayerDefaults(object sender, EventArgs e)
        {
            Rect mbr = (comboLayer.SelectedItem as GeomLayer).MBR;
            numericXMin.Value = (decimal)mbr.XMin;
            numericXMax.Value = (decimal)mbr.XMax;
            numericYMin.Value = (decimal)mbr.YMin;
            numericYMax.Value = (decimal)mbr.YMax;
            ValidateBorders();
        }

        // 运行算法并创建图层
        private void GenGridLayer(object sender, EventArgs e)
        {
            ValidateBorders();

            API.Point2Grid(comboLayer.SelectedItem as GeomLayer, comboMethod.SelectedItem.ToString(),
                (double)numericXMin.Value, (double)numericXMax.Value,
                (double)numericYMin.Value, (double)numericYMax.Value,
                (uint)numericXSplit.Value, (uint)numericYSplit.Value).Add();

            Close();
        }
    }
}
