﻿using MiniGIS.Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniGIS.Widget
{
    public partial class LayerVisibleChecklist : Form
    {
        BaseLayer origin;
        public LayerVisibleChecklist(BaseLayer node)
        {
            InitializeComponent();

            // 绑定图层
            origin = node;
            Text = "可见性: " + node.Name;

            // 插入控制项
            IEnumerable<string> parts = null;
            switch (node)
            {
                case GeomLayer n1:
                    var tmp = new List<string>();
                    if (n1.points != null) tmp.Add("point");
                    if (n1.arcs != null) tmp.Add("arc");
                    if (n1.polygons != null) tmp.Add("polygon");
                    parts = tmp;
                    break;
                case ValueLayer n1:
                    parts = new string[] { "border", "fill" };
                    break;
            }
            if (parts != null)
            {
                visibleParts.ItemCheck -= visibleParts_ItemCheck;
                foreach (string key in parts)
                {
                    visibleParts.Items.Add(key, node.GetPartVisible(key));
                }
                visibleParts.ItemCheck += visibleParts_ItemCheck;
            }
        }

        private void visibleLayer_CheckedChanged(object sender, EventArgs e)
        {
            origin.Visible = visibleLayer.Checked;
            UpdateControls(sender, e);
        }

        private void visibleParts_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            origin.SetPartVisible(visibleParts.Items[e.Index].ToString(), !visibleParts.GetItemChecked(e.Index)); // 更新项勾选值反向
            MainForm.port.Render();
        }

        // 弹出前更新面板可用状态
        private void UpdateControls(object sender, EventArgs e)
        {
            groupVisible.Enabled = visibleLayer.Checked = origin.Visible;
        }
    }
}
