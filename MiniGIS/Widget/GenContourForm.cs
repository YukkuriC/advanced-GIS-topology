﻿using MiniGIS.Render;
using MiniGIS.Algorithm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MiniGIS.Data;

namespace MiniGIS.Widget
{
    public partial class GenContourForm : Form
    {
        IEnumerable<double> targetSplits;
        int N;

        public GenContourForm()
        {
            InitializeComponent();

            // 绑定图层
            var layers = Utils.BindLayers<ValueLayer>(comboLayer);

            // 初状态
            btnGen.Enabled = layers.Count > 0;
        }

        // 输出等分点
        IEnumerable<double> Linear(double start, double step, int s1, int s2)
        {
            for (int i = s1; i <= s2; i++)
                yield return start + step * i;
        }

        // 根据图层生成等分位置
        public void CalcSplits(object sender, EventArgs e)
        {
            ValueLayer layer = comboLayer.SelectedItem as ValueLayer;
            if (layer == null) return;

            // 计算等分位置
            double splitSize = (double)inputSplitSize.Value;
            double splitBase = (double)inputSplitBase.Value;
            int s1 = (int)Math.Ceiling((layer.Min - splitBase) / splitSize);
            int s2 = (int)Math.Floor((layer.Max - splitBase) / splitSize);
            N = Math.Max(s2 - s1 + 1, 0);
            targetSplits = Linear(splitBase, splitSize, s1, s2);
            double sMin = splitBase + splitSize * s1;
            double sMax = splitBase + splitSize * s2;

            // 更新文字
            switch (N)
            {
                case 0: splitPreview.Text = "无"; break;
                case 1: splitPreview.Text = sMin.ToString(); break;
                case 2:
                case 3:
                    splitPreview.Text = String.Join(", ", from i in targetSplits select sMax.SciString(3));
                    break;
                default:
                    splitPreview.Text = String.Format("{0}, [{1}个元素], {2}", sMin.SciString(3), N - 2, sMax.SciString(3));
                    break;
            }
        }

        private void GenContour(object sender, EventArgs e)
        {
            // 不可生成空图层
            if (N == 0)
            {
                MessageBox.Show("未生成任何等值线", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<GeomPoint> points = null;
            List<GeomArc> arcs = null;

            // 执行搜索算法
            switch (comboLayer.SelectedItem)
            {
                case GridLayer layer:
                    Contour.GenContourGrid(layer.data, targetSplits, out points, out arcs);
                    break;
                case TINLayer layer:
                    Contour.GenContourTIN(layer.edgeSides, layer.values, targetSplits, out points, out arcs);
                    break;
                default:
                    MessageBox.Show("图层类型不支持", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            // 创建图层
            var result = new GeomLayer(
                GeomType.Arc,
                (comboLayer.SelectedItem as Layer).Name +
                String.Format("_等值线({0},{1})", (double)inputSplitBase.Value, (double)inputSplitSize.Value)
            )
            {
                points = points,
                arcs = arcs
            };

            // 配置默认样式
            result.colors["arc"] = Color.Orange;
            result.SetPartVisible("point", false);

            // 添加图层
            result.Add();

            // 结束
            Close();
        }
    }
}
