﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MiniGIS.Render;

namespace MiniGIS.Widget
{
    public partial class LayerSettingsGrid : Form, ILayerSettings
    {
        public Layer origin;

        public LayerSettingsGrid()
        {
            InitializeComponent();
        }

        public void BindLayer(Layer layer)
        {
            origin = layer;
            layerName.Text = layer.Name;
        }

        public void BindControls()
        {
            LayerSettings.BindColor(origin, button1, "low", splitContainer1.Panel1);
            LayerSettings.BindColor(origin, button2, "high", splitContainer2.Panel1);
            LayerSettings.BindColor(origin, button3, "grid", splitContainer3.Panel1);
            LayerSettings.BindSize(origin, numericUpDown1, "low");
            LayerSettings.BindSize(origin, numericUpDown2, "high");
            LayerSettings.BindSize(origin, numericUpDown3, "grid");
        }

        private void layerName_TextChanged(object sender, EventArgs e)
        {
            origin.Name = layerName.Text;
            origin.UpdateText();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.Value = Math.Min(numericUpDown1.Value, numericUpDown2.Value - (decimal)0.001);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown2.Value = Math.Max(numericUpDown2.Value, numericUpDown1.Value + (decimal)0.001);
        }
    }
}