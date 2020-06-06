﻿using System;
using System.Windows.Forms;
using MiniGIS.Render;

namespace MiniGIS.Widget
{
    // 图层右键菜单
    class LayerContext : ContextMenuStrip
    {
        Layer origin;

        // 绑定右键菜单至图层
        public LayerContext(Layer node)
        {
            origin = node;
            node.ContextMenuStrip = this;
            Items.AddRange(new ToolStripMenuItem[]{
                _("查看", null,
                    _("聚焦图层", (object s, EventArgs a) => origin.Focus(MainForm.port)),
                    _("可见性设置", (object s, EventArgs a) => new LayerVisibleChecklist(node).ShowDialog())
                ),
                _("调整顺序", null,
                    _("置于顶层", (object s, EventArgs a) => origin.Reorder(0)),
                    _("置于底层", (object s, EventArgs a) => origin.Reorder(int.MaxValue)),
                    _("上移一层", (object s, EventArgs a) => origin.Reorder(origin.Index - 1)),
                    _("下移一层", (object s, EventArgs a) => origin.Reorder(origin.Index + 1))
                ),
                _("删除图层", (object s, EventArgs a) => origin.Remove()),
                _("图层设置", (object s, EventArgs a) =>
                {
                    Form settings = null;
                    switch (node)
                    {
                        case GeomLayer n1: settings = new LayerSettingsGeom(n1); break;
                        case GridLayer n1: settings = new LayerSettingsGrid(n1); break;
                    }
                    if (settings != null) (settings as Form).ShowDialog();
                }),
            });
        }

        // 创建右键菜单helper
        static ToolStripMenuItem _(string text, Action<object, EventArgs> clickEvent = null, params ToolStripMenuItem[] subItems)
        {
            ToolStripMenuItem res = new ToolStripMenuItem { Text = text };
            if (clickEvent != null) res.Click += clickEvent.Invoke;
            if (subItems.Length > 0) res.DropDownItems.AddRange(subItems);
            return res;
        }
    }
}
