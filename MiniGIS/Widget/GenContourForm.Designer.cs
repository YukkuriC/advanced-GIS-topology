namespace MiniGIS.Widget
{
    partial class GenContourForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.comboLayer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.inputSplitSize = new System.Windows.Forms.NumericUpDown();
            this.inputSplitBase = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.splitPreview = new System.Windows.Forms.Label();
            this.btnGen = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inputSplitSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inputSplitBase)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboLayer, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.inputSplitSize, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.inputSplitBase, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.splitPreview, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnGen, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.00049F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0005F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0005F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.9985F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(284, 142);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "图层";
            // 
            // comboLayer
            // 
            this.comboLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLayer.FormattingEnabled = true;
            this.comboLayer.Location = new System.Drawing.Point(74, 4);
            this.comboLayer.Name = "comboLayer";
            this.comboLayer.Size = new System.Drawing.Size(207, 20);
            this.comboLayer.TabIndex = 9;
            this.comboLayer.SelectedIndexChanged += new System.EventHandler(this.CalcSplits);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "间隔";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "分割起点";
            // 
            // inputSplitSize
            // 
            this.inputSplitSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.inputSplitSize.DecimalPlaces = 10;
            this.inputSplitSize.Location = new System.Drawing.Point(74, 31);
            this.inputSplitSize.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.inputSplitSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            655360});
            this.inputSplitSize.Name = "inputSplitSize";
            this.inputSplitSize.Size = new System.Drawing.Size(207, 21);
            this.inputSplitSize.TabIndex = 15;
            this.inputSplitSize.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.inputSplitSize.ValueChanged += new System.EventHandler(this.CalcSplits);
            // 
            // inputSplitBase
            // 
            this.inputSplitBase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.inputSplitBase.DecimalPlaces = 10;
            this.inputSplitBase.Location = new System.Drawing.Point(74, 59);
            this.inputSplitBase.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.inputSplitBase.Minimum = new decimal(new int[] {
            1410065408,
            2,
            0,
            -2147483648});
            this.inputSplitBase.Name = "inputSplitBase";
            this.inputSplitBase.Size = new System.Drawing.Size(207, 21);
            this.inputSplitBase.TabIndex = 19;
            this.inputSplitBase.ValueChanged += new System.EventHandler(this.CalcSplits);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "待分割值";
            // 
            // splitPreview
            // 
            this.splitPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.splitPreview.AutoSize = true;
            this.splitPreview.Location = new System.Drawing.Point(74, 92);
            this.splitPreview.Name = "splitPreview";
            this.splitPreview.Size = new System.Drawing.Size(207, 12);
            this.splitPreview.TabIndex = 21;
            this.splitPreview.Text = "------";
            // 
            // btnGen
            // 
            this.btnGen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.btnGen, 2);
            this.btnGen.Location = new System.Drawing.Point(3, 115);
            this.btnGen.Name = "btnGen";
            this.btnGen.Size = new System.Drawing.Size(278, 23);
            this.btnGen.TabIndex = 22;
            this.btnGen.Text = "生成等值线";
            this.btnGen.UseVisualStyleBackColor = true;
            this.btnGen.Click += new System.EventHandler(this.GenContour);
            // 
            // GenContourForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 142);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenContourForm";
            this.ShowIcon = false;
            this.Text = "生成等值线";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inputSplitSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inputSplitBase)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboLayer;
        private System.Windows.Forms.NumericUpDown inputSplitSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown inputSplitBase;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label splitPreview;
        private System.Windows.Forms.Button btnGen;
    }
}