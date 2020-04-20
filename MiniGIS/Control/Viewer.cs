﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MiniGIS.Control
{
    class Viewer : MapControl
    {
        PointF lastScreen, lastWorld;
        bool dragging = false;
        int zoomLevel = 0;

        public void MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    dragging = true;
                    lastScreen = new PointF(e.X, e.Y);
                    lastWorld = MainForm.port.center;
                    break;

                case MouseButtons.Right:
                    zoomLevel = 0;
                    MainForm.port.Reset();
                    break;
            }
        }

        // 释放拖拽模式
        public void MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    MouseMove(sender, e);
                    dragging = false;
                    break;
            }
        }

        // (拖拽模式下)移动中心点
        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                float dx = (lastScreen.X - e.X) / MainForm.port.zoom,
                      dy = -(lastScreen.Y - e.Y) / MainForm.port.zoom; // 屏幕坐标向下为正
                MainForm.port.center = new PointF(
                    lastWorld.X + dx,
                    lastWorld.Y + dy
                );
                MainForm.port.Render();
            }
        }

        // (非拖拽模式下)以鼠标位置为基准缩放
        public void MouseWheel(object sender, MouseEventArgs e)
        {
            if (dragging) return;

            // 更新缩放等级
            if (e.Delta > 0) zoomLevel += 1;
            else zoomLevel -= 1;
            zoomLevel = Math.Max(Math.Min(zoomLevel, 10), -10);
            float newZoom = (float)Math.Pow(2, zoomLevel);

            // 计算位置偏移
            PointF oldCenter = MainForm.port.center;
            MainForm.port.WorldCoord(e.X, e.Y, out float oldX, out float oldY);
            MainForm.port.zoom = newZoom;
            MainForm.port.WorldCoord(e.X, e.Y, out float newX, out float newY);

            // 更新窗口位置
            MainForm.port.center.X += oldX - newX;
            MainForm.port.center.Y += oldY - newY;
            MainForm.port.Render();
        }
    }
}