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
        #region prop

        // 图层属性
        public virtual string layerType { get { return "图层"; } }
        public string name;

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
        public Color color1 = Color.Red, color2 = Color.Black, color3 = Color.Green, color_annotation = Color.Black;
        public float size_pt = 5, size_arc = 2, size_annotation = 10;

        #endregion

        public virtual void Render(ViewPort port, Graphics canvas) { }

        #region method

        // 更新图层显示
        public void UpdateText()
        {
            // 更新图层名称
            string res = String.Format("[{0}] {1}", layerType, name);
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
            name = _name;
            NodeFont = MainForm.instance.Font;

            // 插入右键菜单
            new LayerContext(this);
        }
    }

    // 图层右键菜单
    class LayerContext : ContextMenuStrip
    {
        ToolStripMenuItem btnToggleVisible, btnMove, btnMoveUp, btnMoveDown, btnDelete;
        Layer origin;

        #region method

        // 控制图层隐藏显示
        public void TriggerVisible(object sender, EventArgs args)
        {
            origin.Visible = !origin.Visible;
            UpdateText();
        }

        // 删除图层
        public void Delete(object sender, EventArgs args)
        {
            origin.Remove();
        }

        // 图层移动
        public void MoveUp(object sender, EventArgs args) { origin.Reorder(origin.Index - 1); }
        public void MoveDown(object sender, EventArgs args) { origin.Reorder(origin.Index + 1); }

        // 更新文本
        public void UpdateText(object sender = null, EventArgs args = null)
        {
            btnToggleVisible.Text = origin.Visible ? "隐藏图层" : "显示图层";
            origin.UpdateText();
        }

        #endregion

        // 绑定右键菜单至图层
        public LayerContext(Layer node)
        {
            origin = node;
            node.ContextMenuStrip = this;
            Items.AddRange(new ToolStripMenuItem[]{
                btnToggleVisible = new ToolStripMenuItem(),
                btnMove = new ToolStripMenuItem { Text = "调整顺序" },
                btnDelete = new ToolStripMenuItem { Text = "删除图层" },
            });
            btnMove.DropDownItems.AddRange(new ToolStripMenuItem[]{
                btnMoveUp = new ToolStripMenuItem { Text = "上移一层" },
                btnMoveDown = new ToolStripMenuItem { Text = "下移一层" },
            });
            btnToggleVisible.Click += TriggerVisible;
            btnDelete.Click += Delete;
            btnMoveUp.Click += MoveUp;
            btnMoveDown.Click += MoveDown;
            UpdateText();
        }
    }
}
