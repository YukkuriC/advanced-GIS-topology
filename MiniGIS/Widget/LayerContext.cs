using System;
using System.Windows.Forms;
using MiniGIS.Render;

namespace MiniGIS.Widget
{
    // 图层右键菜单
    class LayerContext : ContextMenuStrip
    {
        ToolStripMenuItem btnToggleVisible, btnMove, btnFocus, btnMoveUp, btnMoveDown, btnDelete, btnSettings;
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

        // 聚焦图层
        public void Focus(object sender, EventArgs args) { origin.Focus(MainForm.port); }

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
                btnFocus = new ToolStripMenuItem { Text = "聚焦图层" },
                btnDelete = new ToolStripMenuItem { Text = "删除图层" },
                btnSettings = new ToolStripMenuItem { Text = "图层设置" },
            });
            btnMove.DropDownItems.AddRange(new ToolStripMenuItem[]{
                btnMoveUp = new ToolStripMenuItem { Text = "上移一层" },
                btnMoveDown = new ToolStripMenuItem { Text = "下移一层" },
            });
            btnToggleVisible.Click += TriggerVisible;
            btnDelete.Click += Delete;
            btnMoveUp.Click += MoveUp;
            btnMoveDown.Click += MoveDown;
            btnFocus.Click += Focus;
            btnSettings.Click += (object sender, EventArgs args) =>
            {
                if (node is GeomLayer) new LayerSettingsGeom(node).ShowDialog();
                else if (node is GridLayer) new LayerSettingsGrid(node).ShowDialog();
            };
            UpdateText();
        }
    }
}
