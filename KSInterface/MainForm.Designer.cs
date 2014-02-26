namespace KSInterface
{
    partial class MainForm
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
            this.Start = new System.Windows.Forms.Button();
            this.End = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.SampleImages = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(7, 3);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(75, 23);
            this.Start.TabIndex = 0;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // End
            // 
            this.End.Location = new System.Drawing.Point(88, 3);
            this.End.Name = "End";
            this.End.Size = new System.Drawing.Size(75, 23);
            this.End.TabIndex = 1;
            this.End.Text = "End";
            this.End.UseVisualStyleBackColor = true;
            this.End.Click += new System.EventHandler(this.End_Click);
            // 
            // rtbLog
            // 
            this.rtbLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.rtbLog.Location = new System.Drawing.Point(7, 27);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(156, 254);
            this.rtbLog.TabIndex = 2;
            this.rtbLog.Text = "";
            // 
            // SampleImages
            // 
            this.SampleImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.SampleImages.ImageSize = new System.Drawing.Size(16, 16);
            this.SampleImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.End);
            this.splitContainer1.Panel1.Controls.Add(this.rtbLog);
            this.splitContainer1.Panel1.Controls.Add(this.Start);
            this.splitContainer1.Size = new System.Drawing.Size(661, 289);
            this.splitContainer1.SplitterDistance = 168;
            this.splitContainer1.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 313);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "Keystone";
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.Button End;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.ImageList SampleImages;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

