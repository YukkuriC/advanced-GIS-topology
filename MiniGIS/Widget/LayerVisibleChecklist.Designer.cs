namespace MiniGIS.Widget
{
    partial class LayerVisibleChecklist
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
            this.visibleParts = new System.Windows.Forms.CheckedListBox();
            this.visibleLayer = new System.Windows.Forms.CheckBox();
            this.groupVisible = new System.Windows.Forms.GroupBox();
            this.groupVisible.SuspendLayout();
            this.SuspendLayout();
            // 
            // visibleParts
            // 
            this.visibleParts.CheckOnClick = true;
            this.visibleParts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visibleParts.FormattingEnabled = true;
            this.visibleParts.Location = new System.Drawing.Point(3, 17);
            this.visibleParts.Name = "visibleParts";
            this.visibleParts.Size = new System.Drawing.Size(217, 83);
            this.visibleParts.TabIndex = 0;
            this.visibleParts.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.visibleParts_ItemCheck);
            // 
            // visibleLayer
            // 
            this.visibleLayer.AutoSize = true;
            this.visibleLayer.Location = new System.Drawing.Point(13, 13);
            this.visibleLayer.Name = "visibleLayer";
            this.visibleLayer.Size = new System.Drawing.Size(72, 16);
            this.visibleLayer.TabIndex = 1;
            this.visibleLayer.Text = "显示图层";
            this.visibleLayer.UseVisualStyleBackColor = true;
            this.visibleLayer.CheckedChanged += new System.EventHandler(this.visibleLayer_CheckedChanged);
            // 
            // groupVisible
            // 
            this.groupVisible.Controls.Add(this.visibleParts);
            this.groupVisible.Location = new System.Drawing.Point(13, 36);
            this.groupVisible.Name = "groupVisible";
            this.groupVisible.Size = new System.Drawing.Size(223, 103);
            this.groupVisible.TabIndex = 2;
            this.groupVisible.TabStop = false;
            this.groupVisible.Text = "组件可见性";
            // 
            // LayerVisibleChecklist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 151);
            this.Controls.Add(this.groupVisible);
            this.Controls.Add(this.visibleLayer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LayerVisibleChecklist";
            this.Text = "可见性";
            this.Load += new System.EventHandler(this.UpdateControls);
            this.groupVisible.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox visibleParts;
        private System.Windows.Forms.CheckBox visibleLayer;
        private System.Windows.Forms.GroupBox groupVisible;
    }
}