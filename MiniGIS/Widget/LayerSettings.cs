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
    }

    public class LayerSettings
    {
        #region method

        // 绑定标题设置
        public static void BindTitle(Layer origin, TextBox input, Form form)
        {
            input.TextChanged += (object s, EventArgs e) =>
            {
                origin.Name = input.Text;
                origin.UpdateText();
                form.Text = "设置: " + input.Text;
            };
            input.Text = origin.Name;
        }

        // 绑定选择颜色按钮
        public static void BindColor(Layer origin, Button btnChange, string key, Panel neon)
        {
            // 设置初始颜色
            neon.BackColor = origin.GetColor(key);

            // 绑定事件
            btnChange.Click += (object s, EventArgs e) =>
            {
                ColorDialog dlg = new ColorDialog();
                dlg.Color = origin.GetColor(key);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    neon.BackColor = origin.colors[key] = dlg.Color;
                    MainForm.port.Render();
                }
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
