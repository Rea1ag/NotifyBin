
namespace NotifyBin
{
	partial class About
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
			this.Closebutton = new System.Windows.Forms.Button();
			this.Headlinelabel = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.Descriptionlabel = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// Closebutton
			// 
			this.Closebutton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Closebutton.Location = new System.Drawing.Point(264, 3);
			this.Closebutton.Name = "Closebutton";
			this.Closebutton.Size = new System.Drawing.Size(75, 23);
			this.Closebutton.TabIndex = 0;
			this.Closebutton.Text = "Close";
			this.Closebutton.UseVisualStyleBackColor = true;
			this.Closebutton.Click += new System.EventHandler(this.Closebutton_Click);
			// 
			// Headlinelabel
			// 
			this.Headlinelabel.AutoSize = true;
			this.Headlinelabel.Font = new System.Drawing.Font("Georgia", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Headlinelabel.ForeColor = System.Drawing.SystemColors.Highlight;
			this.Headlinelabel.Location = new System.Drawing.Point(54, 19);
			this.Headlinelabel.Name = "Headlinelabel";
			this.Headlinelabel.Size = new System.Drawing.Size(97, 23);
			this.Headlinelabel.TabIndex = 1;
			this.Headlinelabel.Text = "Notify Bin";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 11);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(43, 37);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// Descriptionlabel
			// 
			this.Descriptionlabel.AutoSize = true;
			this.Descriptionlabel.Location = new System.Drawing.Point(55, 55);
			this.Descriptionlabel.Name = "Descriptionlabel";
			this.Descriptionlabel.Size = new System.Drawing.Size(60, 13);
			this.Descriptionlabel.TabIndex = 3;
			this.Descriptionlabel.Text = "Description";
			// 
			// panel1
			// 
			this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.Add(this.Closebutton);
			this.panel1.Location = new System.Drawing.Point(-1, 135);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(346, 31);
			this.panel1.TabIndex = 4;
			// 
			// About
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(344, 168);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.Descriptionlabel);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.Headlinelabel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "About";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "About";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button Closebutton;
		private System.Windows.Forms.Label Headlinelabel;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label Descriptionlabel;
		private System.Windows.Forms.Panel panel1;
	}
}