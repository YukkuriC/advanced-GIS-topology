using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MiniGIS.Widget;

namespace MiniGIS.Render
{
    public class Layer : TreeNode
    {
        #region prop

        // 图层属性
        public virtual string layerType { get { return "图层"; } }
        public int seed;

        // 控制参数
        public Byte visible_mask = 0b111;
        public Byte annotation_mask = 0b000;
        bool _visible = true;
        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                MainForm.port.Render();
            }
        }

        // 渲染样式
        public Dictionary<string, Color> colors;
        public Dictionary<string, float> sizes;

        #endregion

        #region method

        // 渲染基类方法，用于初始化
        public virtual void Render(ViewPort port, Graphics canvas)
        {
            ColorOps.Init(seed);
        }

        // 获取默认参数
        public Color GetColor(string key) { Color res = Color.Empty; colors.TryGetValue(key, out res); return res; }
        public float GetSize(string key) { float res = 0.1f; sizes.TryGetValue(key, out res); return res; }

        // 更新图层显示
        public void UpdateText()
        {
            // 更新图层名称
            string res = String.Format("[{0}] {1}", layerType, Name);
            if (Visible) NodeFont = new Font(NodeFont, FontStyle.Bold);
            else
            {
                res += " (隐藏)";
                NodeFont = new Font(NodeFont, FontStyle.Italic);
            }
            Text = res;
        }

        // 添加图层
        public void Add()
        {
            MainForm.instance.layerView.Nodes.Insert(0, this);
            MainForm.port.Render();
            UpdateText();
        }

        // 移除图层
        public new void Remove()
        {
            base.Remove();
            MainForm.port.Render();
        }

        // 变更次序
        public void Reorder(int i)
        {
            base.Remove();
            MainForm.instance.layerView.Nodes.Insert(i, this);
            MainForm.port.Render();
            UpdateText(); // 强制刷新文本字体更新宽度
        }

        #endregion

        public Layer(string _name = "图层")
        {
            Name = _name;
            NodeFont = MainForm.instance.Font;
            seed = MainForm.random.Next(int.MinValue, int.MaxValue);
            colors = new Dictionary<string, Color>
            {
                ["point"] = Color.Red,
                ["arc"] = Color.Black,
            };
            sizes = new Dictionary<string, float>
            {
                ["point"] = 5,
                ["arc"] = 2,
            };

            // 插入右键菜单
            new LayerContext(this);
        }
    }
}
