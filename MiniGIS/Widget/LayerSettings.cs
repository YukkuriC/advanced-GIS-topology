using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MiniGIS.Render;

namespace MiniGIS.Widget
{
    public interface ILayerSettings
    {
        void BindLayer(Layer layer); // 绑定待控制图层
        void BindControls(); // 数据绑定控件
    }

    public static class LayerSettings
    {
        public static Dictionary<Layer, ILayerSettings> instances;

        // 去重显示图层设置
        public static T ShowSettings<T>(Layer layer) where T : Form, ILayerSettings, new()
        {
            if (instances == null) instances = new Dictionary<Layer, ILayerSettings>();
            if (!instances.TryGetValue(layer, out ILayerSettings form))
                instances[layer] = form = new T();
            form.BindLayer(layer);
            form.BindControls();
            (form as Form).Focus();
            return (T)form;
        }

        #region method

        // 绑定选择颜色按钮
        public static void BindColor(Layer origin, Button btnChange, string key, Panel neon)
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
        public static void BindSize(Layer origin, NumericUpDown input, string key)
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
    }
}
