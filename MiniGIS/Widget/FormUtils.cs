using MiniGIS.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniGIS.Widget
{
    public static class FormUtils
    {
        // 绑定指定类型图层至选框
        public static List<T> BindLayers<T>(ComboBox comboBox, IEnumerable<Layer> origin = null) where T : Layer
        {
            if (origin == null) origin = MainForm.instance.layerView.Nodes.OfType<Layer>();
            List<T> layers = new List<T>(origin.OfType<T>());
            comboBox.DataSource = layers;
            comboBox.DisplayMember = "Text";

            return layers;
        }
    }
}
