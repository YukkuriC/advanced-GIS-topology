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
            this.menuGridInterpolation = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTIN = new System.Windows.Forms.ToolStripDropDownButton();
            this.menuGenTIN = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContour = new System.Windows.Forms.ToolStripDropDownButton();
            this.menuGenContour = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContourSmooth = new System.Windows.Forms.ToolStripMenuItem();
            this.controlSet = new System.Windows.Forms.ToolStripComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.layerView = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rendererPort = new System.Windows.Forms.PictureBox();
            this.labelInfo = new System.Windows.Forms.Label();
            this.menuTopology = new System.Windows.Forms.ToolStripDropDownButton();
            this.menuGenTopology = new System.Windows.Forms.ToolStripMenuItem();
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
            this.menuGridConverter,
            this.menuTIN,
            this.menuContour,
            this.controlSet,
            this.menuTopology});
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
            this.menuGenGrid.Size = new System.Drawing.Size(148, 22);
            this.menuGenGrid.Text = "生成格网模型";
            // 
            // menuGridInterpolation
            // 
            this.menuGridInterpolation.Name = "menuGridInterpolation";
            this.menuGridInterpolation.Size = new System.Drawing.Size(148, 22);
            this.menuGridInterpolation.Text = "格网加密";
            // 
            // menuTIN
            // 
            this.menuTIN.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuTIN.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuGenTIN});
            this.menuTIN.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuTIN.Name = "menuTIN";
            this.menuTIN.Size = new System.Drawing.Size(66, 22);
            this.menuTIN.Text = "TIN模型";
            // 
            // menuGenTIN
            // 
            this.menuGenTIN.Name = "menuGenTIN";
            this.menuGenTIN.Size = new System.Drawing.Size(121, 22);
            this.menuGenTIN.Text = "生成TIN";
            // 
            // menuContour
            // 
            this.menuContour.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuContour.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuGenContour,
            this.menuContourSmooth});
            this.menuContour.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuContour.Name = "menuContour";
            this.menuContour.Size = new System.Drawing.Size(57, 22);
            this.menuContour.Text = "等值线";
            // 
            // menuGenContour
            // 
            this.menuGenContour.Name = "menuGenContour";
            this.menuGenContour.Size = new System.Drawing.Size(152, 22);
            this.menuGenContour.Text = "生成等值线";
            // 
            // menuContourSmooth
            // 
            this.menuContourSmooth.Name = "menuContourSmooth";
            this.menuContourSmooth.Size = new System.Drawing.Size(152, 22);
            this.menuContourSmooth.Text = "等值线平滑";
            // 
            // controlSet
            // 
            this.controlSet.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.controlSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.controlSet.Items.AddRange(new object[] {
            "拖动浏览",
            "对象查看"});
            this.controlSet.Name = "controlSet";
            this.controlSet.Size = new System.Drawing.Size(121, 25);
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
            this.layerView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.UpdateSelection);
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
            // menuTopology
            // 
            this.menuTopology.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuTopology.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuGenTopology});
            this.menuTopology.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuTopology.Name = "menuTopology";
            this.menuTopology.Size = new System.Drawing.Size(69, 22);
            this.menuTopology.Text = "拓扑生成";
            // 
            // menuGenTopology
            // 
            this.menuGenTopology.Name = "menuGenTopology";
            this.menuGenTopology.Size = new System.Drawing.Size(152, 22);
            this.menuGenTopology.Text = "生成拓扑图层";
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
        private System.Windows.Forms.ToolStripDropDownButton menuTIN;
        private System.Windows.Forms.ToolStripMenuItem menuGenTIN;
        private System.Windows.Forms.ToolStripDropDownButton menuContour;
        private System.Windows.Forms.ToolStripMenuItem menuGenContour;
        private System.Windows.Forms.ToolStripMenuItem menuContourSmooth;
        private System.Windows.Forms.ToolStripComboBox controlSet;
        private System.Windows.Forms.ToolStripDropDownButton menuTopology;
        private System.Windows.Forms.ToolStripMenuItem menuGenTopology;
    }
}

