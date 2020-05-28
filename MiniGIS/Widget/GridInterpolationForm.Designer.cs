namespace MiniGIS.Widget
{
    partial class GridInterpolationForm
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
            this.numericXStep = new System.Windows.Forms.NumericUpDown();
            this.numericYStep = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnGen = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericXStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericYStep)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboLayer, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericXStep, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.numericYStep, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnGen, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(282, 108);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "图层";
            // 
            // comboLayer
            // 
            this.comboLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLayer.FormattingEnabled = true;
            this.comboLayer.Location = new System.Drawing.Point(73, 3);
            this.comboLayer.Name = "comboLayer";
            this.comboLayer.Size = new System.Drawing.Size(206, 20);
            this.comboLayer.TabIndex = 8;
            // 
            // numericXStep
            // 
            this.numericXStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericXStep.Location = new System.Drawing.Point(73, 30);
            this.numericXStep.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericXStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericXStep.Name = "numericXStep";
            this.numericXStep.Size = new System.Drawing.Size(206, 21);
            this.numericXStep.TabIndex = 13;
            this.numericXStep.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // numericYStep
            // 
            this.numericYStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericYStep.Location = new System.Drawing.Point(73, 57);
            this.numericYStep.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericYStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericYStep.Name = "numericYStep";
            this.numericYStep.Size = new System.Drawing.Size(206, 21);
            this.numericYStep.TabIndex = 16;
            this.numericYStep.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "X等分数";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 7;
            this.label8.Text = "Y等分数";
            // 
            // btnGen
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnGen, 2);
            this.btnGen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGen.Location = new System.Drawing.Point(3, 84);
            this.btnGen.Name = "btnGen";
            this.btnGen.Size = new System.Drawing.Size(276, 21);
            this.btnGen.TabIndex = 17;
            this.btnGen.Text = "加密格网";
            this.btnGen.UseVisualStyleBackColor = true;
            this.btnGen.Click += new System.EventHandler(this.GenGridLayer);
            // 
            // GridInterpolationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 108);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GridInterpolationForm";
            this.ShowIcon = false;
            this.Text = "生成格网模型";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericXStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericYStep)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboLayer;
        private System.Windows.Forms.NumericUpDown numericYStep;
        private System.Windows.Forms.NumericUpDown numericXStep;
        private System.Windows.Forms.Button btnGen;
    }
}