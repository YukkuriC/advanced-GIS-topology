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
    public partial class LayerSettingsGeom : Form
    {
        public Layer origin;

        public LayerSettingsGeom(GeomLayer layer)
        {
            InitializeComponent();
            origin = layer;
            LayerSettings.BindTitle(layer, layerName, this);
            LayerSettings.BindColor(layer, button1, "point", splitContainer1.Panel1);
            LayerSettings.BindColor(layer, button2, "arc", splitContainer2.Panel1);
            LayerSettings.BindColor(layer, button3, "polygon", splitContainer3.Panel1);
            LayerSettings.BindSize(layer, numericUpDown1, "point");
            LayerSettings.BindSize(layer, numericUpDown2, "arc");

            // 绑定清除多边形颜色功能
            button4.Click += (object o, EventArgs e) => {
                splitContainer3.Panel1.BackColor = origin.colors["polygon"] = Color.Empty;
                MainForm.port.Render();
            };

            // 禁用图层缺失部分
            if (layer.points == null) splitContainer1.Enabled = false;
            if (layer.arcs == null) splitContainer2.Enabled = false;
            if (layer.polygons == null) splitContainer3.Enabled = false;
        }
    }
}
