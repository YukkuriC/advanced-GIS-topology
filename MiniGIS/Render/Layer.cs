using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MiniGIS.Data;

namespace MiniGIS.Render
{
    public class Layer : TreeNode
    {
        // 控制参数
        public bool visible = true;
        public Byte visible_mask = 0b111;
        public Byte annotation_mask = 0b000;

        // 渲染样式
        public Color color1 = Color.Red, color2 = Color.Black, color3 = Color.Green, color_annotation = Color.Black;
        public float size_pt = 5, size_arc = 2, size_annotation = 10;

        public virtual void Render(ViewPort port, Graphics canvas) { }

        // 添加图层
        public void Add()
        {
            MainForm.instance.layerView.Nodes.Add(this);
            MainForm.port.Render();
        }

        // 移除图层
        public void Delete()
        {
            MainForm.instance.layerView.Nodes.Remove(this);
            MainForm.port.Render();
        }

        public Layer(string name = "图层")
        {
            Text = name;
        }
    }
}
