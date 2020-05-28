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
            this.menuGridConverter = new System.Windows.Forms.ToolStripDropDownButton();
            this.menuGenGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.layerView = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rendererPort = new System.Windows.Forms.PictureBox();
            this.labelInfo = new System.Windows.Forms.Label();
            this.menuGridInterpolation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rendererPort)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLoad,
            this.menuGridConverter});
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
            this.menuLoadCSV.Size = new System.Drawing.Size(123, 22);
            this.menuLoadCSV.Text = "导入CSV";
            this.menuLoadCSV.Click += new System.EventHandler(this.menuLoadCSV_Click);
            // 
            // menuGridConverter
            // 
            this.menuGridConverter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuGridConverter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuGenGrid,
            this.menuGridInterpolation});
            this.menuGridConverter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuGridConverter.Name = "menuGridConverter";
            this.menuGridConverter.Size = new System.Drawing.Size(69, 22);
            this.menuGridConverter.Text = "格网模型";
            // 
            // menuGenGrid
            // 
            this.menuGenGrid.Name = "menuGenGrid";
            this.menuGenGrid.Size = new System.Drawing.Size(152, 22);
            this.menuGenGrid.Text = "生成格网模型";
            this.menuGenGrid.Click += new System.EventHandler(this.menuGenGrid_Click);
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
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(287, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(639, 479);
            this.panel2.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.rendererPort, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelInfo, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(635, 475);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // rendererPort
            // 
            this.rendererPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rendererPort.Location = new System.Drawing.Point(3, 3);
            this.rendererPort.Name = "rendererPort";
            this.rendererPort.Size = new System.Drawing.Size(629, 453);
            this.rendererPort.TabIndex = 0;
            this.rendererPort.TabStop = false;
            this.rendererPort.SizeChanged += new System.EventHandler(this.rendererPort_SizeChanged);
            // 
            // labelInfo
            // 
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo.Location = new System.Drawing.Point(3, 459);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(629, 16);
            this.labelInfo.TabIndex = 1;
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // menuGridInterpolation
            // 
            this.menuGridInterpolation.Name = "menuGridInterpolation";
            this.menuGridInterpolation.Size = new System.Drawing.Size(152, 22);
            this.menuGridInterpolation.Text = "格网加密";
            this.menuGridInterpolation.Click += new System.EventHandler(this.menuGridInterpolation_Click);
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
            this.tableLayoutPanel1.ResumeLayout(false);
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.ToolStripDropDownButton menuGridConverter;
        private System.Windows.Forms.ToolStripMenuItem menuGenGrid;
        private System.Windows.Forms.ToolStripMenuItem menuGridInterpolation;
    }
}

