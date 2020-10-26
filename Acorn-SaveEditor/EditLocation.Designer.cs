namespace Acorn_SaveEditor
{
    partial class EditLocation
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.worldNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.sectionNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nodeNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.worldNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nodeNumUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "World:";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // worldNumUpDown
            // 
            this.worldNumUpDown.Location = new System.Drawing.Point(63, 12);
            this.worldNumUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.worldNumUpDown.Name = "worldNumUpDown";
            this.worldNumUpDown.Size = new System.Drawing.Size(120, 20);
            this.worldNumUpDown.TabIndex = 15;
            // 
            // sectionNumUpDown
            // 
            this.sectionNumUpDown.Location = new System.Drawing.Point(63, 38);
            this.sectionNumUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.sectionNumUpDown.Name = "sectionNumUpDown";
            this.sectionNumUpDown.Size = new System.Drawing.Size(120, 20);
            this.sectionNumUpDown.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Section:";
            // 
            // nodeNumUpDown
            // 
            this.nodeNumUpDown.Location = new System.Drawing.Point(63, 64);
            this.nodeNumUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nodeNumUpDown.Name = "nodeNumUpDown";
            this.nodeNumUpDown.Size = new System.Drawing.Size(120, 20);
            this.nodeNumUpDown.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Node:";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(63, 90);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 20;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // EditLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(198, 119);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.nodeNumUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.sectionNumUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.worldNumUpDown);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditLocation";
            this.ShowIcon = false;
            this.Text = "Edit Location";
            ((System.ComponentModel.ISupportInitialize)(this.worldNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nodeNumUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.NumericUpDown worldNumUpDown;
        private System.Windows.Forms.NumericUpDown sectionNumUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nodeNumUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button okButton;
    }
}