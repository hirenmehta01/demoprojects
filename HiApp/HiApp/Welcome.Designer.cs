namespace HiApp
{
    partial class Welcome
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
            this.mfaq = new MetroFramework.Controls.MetroTile();
            this.mchat = new MetroFramework.Controls.MetroTile();
            this.mCategory = new MetroFramework.Controls.MetroTile();
            this.mAdmin = new MetroFramework.Controls.MetroTile();
            this.SuspendLayout();
            // 
            // mfaq
            // 
            this.mfaq.ActiveControl = null;
            this.mfaq.Location = new System.Drawing.Point(133, 108);
            this.mfaq.Name = "mfaq";
            this.mfaq.Size = new System.Drawing.Size(118, 83);
            this.mfaq.TabIndex = 0;
            this.mfaq.Text = "FAQ";
            this.mfaq.UseSelectable = true;
            // 
            // mchat
            // 
            this.mchat.ActiveControl = null;
            this.mchat.Location = new System.Drawing.Point(274, 110);
            this.mchat.Name = "mchat";
            this.mchat.Size = new System.Drawing.Size(118, 83);
            this.mchat.TabIndex = 1;
            this.mchat.Text = "Chat";
            this.mchat.UseSelectable = true;
            this.mchat.Click += new System.EventHandler(this.mchat_Click);
            // 
            // mCategory
            // 
            this.mCategory.ActiveControl = null;
            this.mCategory.Location = new System.Drawing.Point(273, 199);
            this.mCategory.Name = "mCategory";
            this.mCategory.Size = new System.Drawing.Size(118, 83);
            this.mCategory.TabIndex = 3;
            this.mCategory.Text = "Category";
            this.mCategory.UseSelectable = true;
            // 
            // mAdmin
            // 
            this.mAdmin.ActiveControl = null;
            this.mAdmin.Location = new System.Drawing.Point(132, 197);
            this.mAdmin.Name = "mAdmin";
            this.mAdmin.Size = new System.Drawing.Size(118, 83);
            this.mAdmin.TabIndex = 2;
            this.mAdmin.Text = "Administration";
            this.mAdmin.UseSelectable = true;
            // 
            // Welcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 373);
            this.Controls.Add(this.mCategory);
            this.Controls.Add(this.mAdmin);
            this.Controls.Add(this.mchat);
            this.Controls.Add(this.mfaq);
            this.Name = "Welcome";
            this.Text = "Welcome To HI";
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTile mfaq;
        private MetroFramework.Controls.MetroTile mchat;
        private MetroFramework.Controls.MetroTile mCategory;
        private MetroFramework.Controls.MetroTile mAdmin;
    }
}