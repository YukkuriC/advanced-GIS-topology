namespace MiniGIS.Widget
{
    partial class CSVLoader
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelLoader = new System.Windows.Forms.GroupBox();
            this.filePath = new System.Windows.Forms.Label();
            this.btnOpen = new System.Windows.Forms.Button();
            this.openCSV = new System.Windows.Forms.OpenFileDialog();
            this.panelSettings = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.selectName = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.readHeader = new System.Windows.Forms.CheckBox();
            this.tableInfo = new System.Windows.Forms.Label();
            this.selectID = new System.Windows.Forms.ComboBox();
            this.ID = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.selectValue = new System.Windows.Forms.ComboBox();
            this.selectY = new System.Windows.Forms.ComboBox();
            this.selectX = new System.Windows.Forms.ComboBox();
            this.panelLoader.SuspendLayout();
            this.panelSettings.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLoader
            // 
            this.panelLoader.Controls.Add(this.filePath);
            this.panelLoader.Controls.Add(this.btnOpen);
            this.panelLoader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLoader.Location = new System.Drawing.Point(0, 0);
            this.panelLoader.Name = "panelLoader";
            this.panelLoader.Size = new System.Drawing.Size(236, 44);
            this.panelLoader.TabIndex = 1;
            this.panelLoader.TabStop = false;
            this.panelLoader.Text = "已读取文件";
            // 
            // filePath
            // 
            this.filePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePath.AutoEllipsis = true;
            this.filePath.Location = new System.Drawing.Point(6, 17);
            this.filePath.Name = "filePath";
            this.filePath.Size = new System.Drawing.Size(164, 19);
            this.filePath.TabIndex = 3;
            this.filePath.Text = "无";
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpen.Location = new System.Drawing.Point(176, 11);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(55, 25);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "打开";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.OpenCSV);
            // 
            // openCSV
            // 
            this.openCSV.Filter = "CSV文件|*.csv;*.txt|所有文件|*.*";
            // 
            // panelSettings
            // 
            this.panelSettings.Controls.Add(this.tableLayoutPanel1);
            this.panelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSettings.Enabled = false;
            this.panelSettings.Location = new System.Drawing.Point(0, 44);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(236, 212);
            this.panelSettings.TabIndex = 2;
            this.panelSettings.TabStop = false;
            this.panelSettings.Text = "设置";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.03239F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.96761F));
            this.tableLayoutPanel1.Controls.Add(this.selectName, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnAdd, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.readHeader, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableInfo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.selectID, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ID, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.selectValue, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.selectY, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.selectX, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(230, 192);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // selectName
            // 
            this.selectName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.selectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectName.FormattingEnabled = true;
            this.selectName.Location = new System.Drawing.Point(85, 57);
            this.selectName.Name = "selectName";
            this.selectName.Size = new System.Drawing.Size(142, 20);
            this.selectName.TabIndex = 7;
            // 
            // btnAdd
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnAdd, 2);
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.Location = new System.Drawing.Point(3, 165);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(224, 24);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "插入图层";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.ImportPoints);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Y坐标";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "X坐标";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "名称";
            // 
            // readHeader
            // 
            this.readHeader.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.readHeader.AutoSize = true;
            this.readHeader.Location = new System.Drawing.Point(3, 5);
            this.readHeader.Name = "readHeader";
            this.readHeader.Size = new System.Drawing.Size(72, 16);
            this.readHeader.TabIndex = 0;
            this.readHeader.Text = "包含表头";
            this.readHeader.UseVisualStyleBackColor = true;
            this.readHeader.CheckedChanged += new System.EventHandler(this.UpdateFields);
            // 
            // tableInfo
            // 
            this.tableInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableInfo.Location = new System.Drawing.Point(85, 6);
            this.tableInfo.Name = "tableInfo";
            this.tableInfo.Size = new System.Drawing.Size(142, 15);
            this.tableInfo.TabIndex = 1;
            this.tableInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // selectID
            // 
            this.selectID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.selectID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectID.FormattingEnabled = true;
            this.selectID.Location = new System.Drawing.Point(85, 30);
            this.selectID.Name = "selectID";
            this.selectID.Size = new System.Drawing.Size(142, 20);
            this.selectID.TabIndex = 1;
            // 
            // ID
            // 
            this.ID.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ID.AutoSize = true;
            this.ID.Location = new System.Drawing.Point(3, 34);
            this.ID.Name = "ID";
            this.ID.Size = new System.Drawing.Size(17, 12);
            this.ID.TabIndex = 2;
            this.ID.Text = "ID";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "取值";
            // 
            // selectValue
            // 
            this.selectValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.selectValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectValue.FormattingEnabled = true;
            this.selectValue.Location = new System.Drawing.Point(85, 138);
            this.selectValue.Name = "selectValue";
            this.selectValue.Size = new System.Drawing.Size(142, 20);
            this.selectValue.TabIndex = 1;
            // 
            // selectY
            // 
            this.selectY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.selectY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectY.FormattingEnabled = true;
            this.selectY.Location = new System.Drawing.Point(85, 111);
            this.selectY.Name = "selectY";
            this.selectY.Size = new System.Drawing.Size(142, 20);
            this.selectY.TabIndex = 1;
            // 
            // selectX
            // 
            this.selectX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.selectX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectX.FormattingEnabled = true;
            this.selectX.Location = new System.Drawing.Point(85, 84);
            this.selectX.Name = "selectX";
            this.selectX.Size = new System.Drawing.Size(142, 20);
            this.selectX.TabIndex = 1;
            // 
            // CSVLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 256);
            this.Controls.Add(this.panelSettings);
            this.Controls.Add(this.panelLoader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CSVLoader";
            this.Text = "CSVLoader";
            this.panelLoader.ResumeLayout(false);
            this.panelSettings.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox panelLoader;
        private System.Windows.Forms.Label filePath;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.OpenFileDialog openCSV;
        private System.Windows.Forms.GroupBox panelSettings;
        private System.Windows.Forms.CheckBox readHeader;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ComboBox selectValue;
        private System.Windows.Forms.ComboBox selectY;
        private System.Windows.Forms.ComboBox selectX;
        private System.Windows.Forms.Label tableInfo;
        private System.Windows.Forms.ComboBox selectID;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox selectName;
    }
}