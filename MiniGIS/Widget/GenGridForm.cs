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
            var layers = Utils.BindLayers<GeomLayer>(comboLayer);

            // 初状态
            comboMethod.SelectedIndex = 0;
            btnGen.Enabled = btnReset.Enabled = layers.Count > 0;
        }

        // 读取图层边界，设置默认值
        private void SetLayerDefaults(object sender, EventArgs e)
        {
            Rect mbr = (comboLayer.SelectedItem as GeomLayer).MBR;
            numericXMin.Value = (decimal)mbr.xMin;
            numericXMax.Value = (decimal)mbr.xMax;
            numericYMin.Value = (decimal)mbr.yMin;
            numericYMax.Value = (decimal)mbr.yMax;
            ValidateBorders();
        }

        // 运行算法并创建图层
        private void GenGridLayer(object sender, EventArgs e)
        {
            ValidateBorders();
            GeomLayer layer = comboLayer.SelectedItem as GeomLayer;
            Grid result = new Grid(
                (double)numericXMin.Value, (double)numericXMax.Value,
                (double)numericYMin.Value, (double)numericYMax.Value,
                (uint)numericXSplit.Value, (uint)numericYSplit.Value
            );
            switch (comboMethod.SelectedIndex)
            {
                case 0: result.GenGrid_DirGrouped(layer.points); break;// 方位加权
                case 1: result.GenGrid_DistRev(layer.points); break;// 距离倒数
                case 2: result.GenGrid_DistPow2Rev(layer.points); break;// 距离平方倒数
            }

            // 创建图层
            new GridLayer(result, layer.Name + "_" + comboMethod.SelectedItem.ToString()).Add().Focus(MainForm.port);
            Close();
        }
    }
}
