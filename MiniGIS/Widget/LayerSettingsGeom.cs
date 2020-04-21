using System;
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
    public partial class LayerSettingsGeom : Form, ILayerSettings
    {
        public Layer origin;

        public LayerSettingsGeom()
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
            LayerSettings.BindColor(origin, button1, "point", splitContainer1.Panel1);
            LayerSettings.BindColor(origin, button2, "arc", splitContainer2.Panel1);
            LayerSettings.BindColor(origin, button3, "polygon", splitContainer3.Panel1);
            LayerSettings.BindSize(origin, numericUpDown1, "point");
            LayerSettings.BindSize(origin, numericUpDown2, "arc");
        }

        private void layerName_TextChanged(object sender, EventArgs e)
        {
            origin.Name = layerName.Text;
            origin.UpdateText();
        }
    }
}
