namespace MiniGIS.Widget
{
    partial class GenGridForm
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
            this.btnReset = new System.Windows.Forms.Button();
            this.numericYSplit = new System.Windows.Forms.NumericUpDown();
            this.numericYMax = new System.Windows.Forms.NumericUpDown();
            this.numericYMin = new System.Windows.Forms.NumericUpDown();
            this.numericXSplit = new System.Windows.Forms.NumericUpDown();
            this.numericXMax = new System.Windows.Forms.NumericUpDown();
            this.numericXMin = new System.Windows.Forms.NumericUpDown();
            this.comboMethod = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboLayer = new System.Windows.Forms.ComboBox();
            this.btnGen = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericYSplit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericYMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericYMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericXSplit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericXMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericXMin)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.btnReset, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.numericYSplit, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.numericYMax, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.numericYMin, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.numericXSplit, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.numericXMax, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.numericXMin, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.comboMethod, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.comboLayer, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnGen, 0, 8);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(283, 272);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnReset
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnReset, 2);
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReset.Location = new System.Drawing.Point(143, 243);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(137, 26);
            this.btnReset.TabIndex = 18;
            this.btnReset.Text = "恢复默认值";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.SetLayerDefaults);
            // 
            // numericYSplit
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.numericYSplit, 3);
            this.numericYSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericYSplit.Location = new System.Drawing.Point(73, 213);
            this.numericYSplit.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericYSplit.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericYSplit.Name = "numericYSplit";
            this.numericYSplit.Size = new System.Drawing.Size(207, 21);
            this.numericYSplit.TabIndex = 16;
            this.numericYSplit.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // numericYMax
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.numericYMax, 3);
            this.numericYMax.DecimalPlaces = 10;
            this.numericYMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericYMax.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericYMax.Location = new System.Drawing.Point(73, 183);
            this.numericYMax.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.numericYMax.Minimum = new decimal(new int[] {
            1410065408,
            2,
            0,
            -2147483648});
            this.numericYMax.Name = "numericYMax";
            this.numericYMax.Size = new System.Drawing.Size(207, 21);
            this.numericYMax.TabIndex = 15;
            this.numericYMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // numericYMin
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.numericYMin, 3);
            this.numericYMin.DecimalPlaces = 10;
            this.numericYMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericYMin.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericYMin.Location = new System.Drawing.Point(73, 153);
            this.numericYMin.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.numericYMin.Minimum = new decimal(new int[] {
            1410065408,
            2,
            0,
            -2147483648});
            this.numericYMin.Name = "numericYMin";
            this.numericYMin.Size = new System.Drawing.Size(207, 21);
            this.numericYMin.TabIndex = 14;
            this.numericYMin.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // numericXSplit
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.numericXSplit, 3);
            this.numericXSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericXSplit.Location = new System.Drawing.Point(73, 123);
            this.numericXSplit.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericXSplit.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericXSplit.Name = "numericXSplit";
            this.numericXSplit.Size = new System.Drawing.Size(207, 21);
            this.numericXSplit.TabIndex = 13;
            this.numericXSplit.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // numericXMax
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.numericXMax, 3);
            this.numericXMax.DecimalPlaces = 10;
            this.numericXMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericXMax.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericXMax.Location = new System.Drawing.Point(73, 93);
            this.numericXMax.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.numericXMax.Minimum = new decimal(new int[] {
            1410065408,
            2,
            0,
            -2147483648});
            this.numericXMax.Name = "numericXMax";
            this.numericXMax.Size = new System.Drawing.Size(207, 21);
            this.numericXMax.TabIndex = 11;
            this.numericXMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // numericXMin
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.numericXMin, 3);
            this.numericXMin.DecimalPlaces = 10;
            this.numericXMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericXMin.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericXMin.Location = new System.Drawing.Point(73, 63);
            this.numericXMin.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.numericXMin.Minimum = new decimal(new int[] {
            1410065408,
            2,
            0,
            -2147483648});
            this.numericXMin.Name = "numericXMin";
            this.numericXMin.Size = new System.Drawing.Size(207, 21);
            this.numericXMin.TabIndex = 10;
            this.numericXMin.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // comboMethod
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.comboMethod, 3);
            this.comboMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMethod.FormattingEnabled = true;
            this.comboMethod.Items.AddRange(new object[] {
            "方位取点加权法",
            "距离倒数法",
            "距离平方倒数法"});
            this.comboMethod.Location = new System.Drawing.Point(73, 33);
            this.comboMethod.Name = "comboMethod";
            this.comboMethod.Size = new System.Drawing.Size(207, 20);
            this.comboMethod.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 219);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 7;
            this.label8.Text = "Y等分数";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "图层";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "生成方式";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "X最小值";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "X最大值";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "X等分数";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 159);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "Y最小值";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 189);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "Y最大值";
            // 
            // comboLayer
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.comboLayer, 3);
            this.comboLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLayer.FormattingEnabled = true;
            this.comboLayer.Location = new System.Drawing.Point(73, 3);
            this.comboLayer.Name = "comboLayer";
            this.comboLayer.Size = new System.Drawing.Size(207, 20);
            this.comboLayer.TabIndex = 8;
            this.comboLayer.SelectedIndexChanged += new System.EventHandler(this.SetLayerDefaults);
            // 
            // btnGen
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnGen, 2);
            this.btnGen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGen.Location = new System.Drawing.Point(3, 243);
            this.btnGen.Name = "btnGen";
            this.btnGen.Size = new System.Drawing.Size(134, 26);
            this.btnGen.TabIndex = 17;
            this.btnGen.Text = "生成格网模型";
            this.btnGen.UseVisualStyleBackColor = true;
            this.btnGen.Click += new System.EventHandler(this.GenGridLayer);
            // 
            // GenGridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 272);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenGridForm";
            this.ShowIcon = false;
            this.Text = "生成格网模型";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericYSplit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericYMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericYMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericXSplit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericXMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericXMin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboMethod;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboLayer;
        private System.Windows.Forms.NumericUpDown numericXMin;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.NumericUpDown numericYSplit;
        private System.Windows.Forms.NumericUpDown numericYMax;
        private System.Windows.Forms.NumericUpDown numericYMin;
        private System.Windows.Forms.NumericUpDown numericXSplit;
        private System.Windows.Forms.NumericUpDown numericXMax;
        private System.Windows.Forms.Button btnGen;
    }
}