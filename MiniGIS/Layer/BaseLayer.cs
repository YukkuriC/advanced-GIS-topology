using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MiniGIS.Widget;

namespace MiniGIS.Layer
{
    public class BaseLayer : TreeNode
    {
        #region prop

        // 图层属性
        public virtual string layerType { get { return "图层"; } }
        public int seed;
        public LayerTag tag = 0;

        // 控制参数
        bool _visible = true;
        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                UpdateText();
                MainForm.port.Render();
            }
        }
        Dictionary<string, bool> _visibleParts;
        public bool GetPartVisible(string key) => !_visibleParts.ContainsKey(key) || _visibleParts[key]; // 默认全部可见
        public void SetPartVisible(string key, bool val) { _visibleParts[key] = val; }

        // 渲染样式
        public Dictionary<string, Color> colors;
        public Dictionary<string, float> sizes;

        #endregion

        #region method

        // 在图层列表中选中
        public void GetSelected() => MainForm.instance.layerView.SelectedNode = this;

        // 渲染基类方法，用于初始化
        public virtual void Render(ViewPort port, Graphics canvas) { }

        // 聚焦至该图层
        public void Focus() => Focus(MainForm.port);
        public virtual void Focus(ViewPort port) { }

        // 获取默认参数
        public Color GetColor(string key) { Color res = Color.Empty; colors.TryGetValue(key, out res); return res; }
        public float GetSize(string key) { float res = 0.1f; sizes.TryGetValue(key, out res); return res; }

        // 更新图层显示
        public void UpdateText()
        {
            // 更新字体
            var style = FontStyle.Regular;
            if (!Visible) style |= FontStyle.Italic;
            else
            {
                style |= FontStyle.Bold;
                if (IsSelected) style |= FontStyle.Underline;
            }
            NodeFont = new Font(NodeFont, style);

            // 更新图层名称
            string res = String.Format("[{0}] {1}", layerType, Name);
            if (!Visible) res += " (隐藏)";
            if (IsSelected) res = "*" + res;
            Text = res;
        }

        // 添加图层并设置Tag
        public BaseLayer Add(LayerTag _tag)
        {
            tag = _tag;
            MainForm.instance.layerView.Nodes.Insert(0, this);
            UpdateText();
            GetSelected();
            Focus(MainForm.port);
            return this;
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
            GetSelected();
            MainForm.port.Render();
            UpdateText(); // 强制刷新文本字体更新宽度
        }

        #endregion

        public BaseLayer(string _name = "图层")
        {
            Name = _name;
            NodeFont = MainForm.instance.Font;
            seed = MainForm.random.Next(int.MinValue, int.MaxValue);
            _visibleParts = new Dictionary<string, bool>();
            colors = new Dictionary<string, Color>
            {
                ["point"] = Color.Red,
                ["arc"] = Color.Black,
                ["low"] = Color.Blue,
                ["high"] = Color.Yellow,
                ["grid"] = Color.Black,
            };
            sizes = new Dictionary<string, float>
            {
                ["point"] = 5,
                ["arc"] = 2,
                ["grid"] = 1,
            };

            // 插入右键菜单
            new LayerContext(this);
        }
    }

    // 栅格图层与TIN图层通用父类
    public abstract class ValueLayer : BaseLayer
    {
        public abstract double Max { get; }
        public abstract double Min { get; }
        public ValueLayer(string name = "数值图层") : base(name) { }
    }
}
