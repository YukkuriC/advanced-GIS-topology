﻿using MiniGIS.Algorithm;
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
    public partial class GenTopoForm : Form
    {
        public GenTopoForm()
        {
            InitializeComponent();

            // 绑定图层
            var layers = FormUtils.BindLayers<GeomLayer>(comboLayer, LayerTag.Grid | LayerTag.Contour);

            // 初状态
            btnGen.Enabled = layers.Count > 0;
        }

        private void GenTopo(object sender, EventArgs e)
        {
            var layer = API.Arc2Topology(comboLayer.SelectedItem as GeomLayer);
            layer.Add(LayerTag.Topo);

            // TODO: 默认样式

            Close();
        }
    }
}
