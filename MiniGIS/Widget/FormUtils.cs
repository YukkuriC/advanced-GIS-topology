using MiniGIS.Layer;
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
        public static List<T> BindLayers<T>(ComboBox comboBox, LayerTag targetTag) where T : BaseLayer
        {
            List<T> layers = (
                from layer 
                in MainForm.instance.layerView.Nodes.OfType<T>()
                where (layer.tag-targetTag)==(int)(layer.tag^targetTag)
                select layer
            ).ToList();
            comboBox.DataSource = layers;
            comboBox.DisplayMember = "Text";

            return layers;
        }
    }
}
