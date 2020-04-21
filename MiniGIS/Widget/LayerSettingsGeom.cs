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
        public static Dictionary<Layer, LayerSettingsGeom> instances;

        public static LayerSettingsGeom ShowSettings(Layer layer)
        {
            if (instances == null) instances = new Dictionary<Layer, LayerSettingsGeom>();
            LayerSettingsGeom form;
            if (!instances.TryGetValue(layer, out form))
                instances[layer] = form = new LayerSettingsGeom(layer);
            form.Focus();
            return form;
        }

        #region method

        // 绑定选择颜色按钮
        public void BindColor(Button btnChange, string key, Panel neon)
        {
            // 设置初始颜色
            neon.BackColor = origin.GetColor(key);

            // 绑定事件
            btnChange.Click += (object s, EventArgs e) =>
            {
                ColorDialog dlg = new ColorDialog();
                Color clr = Color.Empty;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    clr = dlg.Color;
                }
                neon.BackColor = origin.colors[key] = dlg.Color;
                MainForm.port.Render();
            };
        }

        // 绑定设置大小控件
        public void BindSize(NumericUpDown input, string key)
        {
            // 设置初始数值
            input.Value = (decimal)origin.GetSize(key);

            // 绑定更新事件
            input.ValueChanged += (object s, EventArgs e) =>
            {
                origin.sizes[key] = (float)input.Value;
                MainForm.port.Render();
            };
        }

        #endregion

        public LayerSettingsGeom(Layer layer)
        {
            InitializeComponent();
            origin = layer;

            layerName.Text = layer.Name;

            // 绑定数据
            BindColor(button1, "point", splitContainer1.Panel1);
            BindColor(button2, "arc", splitContainer2.Panel1);
            BindColor(button3, "polygon", splitContainer3.Panel1);
            BindSize(numericUpDown1, "point");
            BindSize(numericUpDown2, "arc");
        }

        private void layerName_TextChanged(object sender, EventArgs e)
        {
            origin.Name = layerName.Text;
            origin.UpdateText();
        }
    }
}
