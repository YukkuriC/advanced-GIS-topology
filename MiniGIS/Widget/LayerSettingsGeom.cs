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

        public LayerSettingsGeom(Layer layer)
        {
            InitializeComponent();
            BindLayer(layer);
        }

        public void BindLayer(Layer layer)
        {
            origin = layer;
            LayerSettings.BindTitle(layer, layerName, this);
            LayerSettings.BindColor(layer, button1, "point", splitContainer1.Panel1);
            LayerSettings.BindColor(layer, button2, "arc", splitContainer2.Panel1);
            LayerSettings.BindColor(layer, button3, "polygon", splitContainer3.Panel1);
            LayerSettings.BindSize(layer, numericUpDown1, "point");
            LayerSettings.BindSize(layer, numericUpDown2, "arc");
        }
    }
}
