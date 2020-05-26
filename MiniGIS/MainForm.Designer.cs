namespace MiniGIS
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.menuLoad = new System.Windows.Forms.ToolStripDropDownButton();
            this.menuLoadCSV = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.layerView = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rendererPort = new System.Windows.Forms.PictureBox();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rendererPort)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLoad});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(926, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // menuLoad
            // 
            this.menuLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuLoad.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLoadCSV});
            this.menuLoad.Name = "menuLoad";
            this.menuLoad.Size = new System.Drawing.Size(45, 22);
            this.menuLoad.Text = "加载";
            // 
            // menuLoadCSV
            // 
            this.menuLoadCSV.Name = "menuLoadCSV";
            this.menuLoadCSV.Size = new System.Drawing.Size(152, 22);
            this.menuLoadCSV.Text = "导入CSV";
            this.menuLoadCSV.Click += new System.EventHandler(this.menuLoadCSV_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.layerView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 479);
            this.panel1.TabIndex = 1;
            // 
            // layerView
            // 
            this.layerView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layerView.Location = new System.Drawing.Point(0, 0);
            this.layerView.Name = "layerView";
            this.layerView.Size = new System.Drawing.Size(284, 479);
            this.layerView.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(284, 25);
            this.splitter1.MinExtra = 300;
            this.splitter1.MinSize = 150;
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 479);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.rendererPort);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(287, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(639, 479);
            this.panel2.TabIndex = 3;
            // 
            // rendererPort
            // 
            this.rendererPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rendererPort.Location = new System.Drawing.Point(0, 0);
            this.rendererPort.Name = "rendererPort";
            this.rendererPort.Size = new System.Drawing.Size(635, 475);
            this.rendererPort.TabIndex = 0;
            this.rendererPort.TabStop = false;
            this.rendererPort.SizeChanged += new System.EventHandler(this.rendererPort_SizeChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 504);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Topology Mini";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rendererPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox rendererPort;
        public System.Windows.Forms.TreeView layerView;
        private System.Windows.Forms.ToolStripDropDownButton menuLoad;
        private System.Windows.Forms.ToolStripMenuItem menuLoadCSV;
    }
}

