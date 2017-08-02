namespace HiApp
{
    partial class ChatForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatForm));
            this.rchTxtChatShow = new System.Windows.Forms.RichTextBox();
            this.rchtxtChat = new System.Windows.Forms.RichTextBox();
            this.mSend = new MetroFramework.Controls.MetroLink();
            this.mBack = new MetroFramework.Controls.MetroLink();
            this.trayNotifier = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rchTxtChatShow
            // 
            this.rchTxtChatShow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rchTxtChatShow.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rchTxtChatShow.Enabled = false;
            this.rchTxtChatShow.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rchTxtChatShow.Location = new System.Drawing.Point(-2, 77);
            this.rchTxtChatShow.Name = "rchTxtChatShow";
            this.rchTxtChatShow.Size = new System.Drawing.Size(375, 409);
            this.rchTxtChatShow.TabIndex = 2;
            this.rchTxtChatShow.Text = "";
            // 
            // rchtxtChat
            // 
            this.rchtxtChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rchtxtChat.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.rchtxtChat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rchtxtChat.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rchtxtChat.Location = new System.Drawing.Point(-1, 490);
            this.rchtxtChat.Name = "rchtxtChat";
            this.rchtxtChat.Size = new System.Drawing.Size(331, 34);
            this.rchtxtChat.TabIndex = 3;
            this.rchtxtChat.Text = "";
            // 
            // mSend
            // 
            this.mSend.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mSend.Image = global::HiApp.Properties.Resources.if_user_comment_1902264;
            this.mSend.ImageSize = 32;
            this.mSend.Location = new System.Drawing.Point(327, 492);
            this.mSend.Name = "mSend";
            this.mSend.Size = new System.Drawing.Size(46, 32);
            this.mSend.TabIndex = 4;
            this.mSend.UseSelectable = true;
            this.mSend.Click += new System.EventHandler(this.mSend_Click);
            // 
            // mBack
            // 
            this.mBack.Image = global::HiApp.Properties.Resources.if_circle_back_arrow_glyph_763477;
            this.mBack.ImageSize = 32;
            this.mBack.Location = new System.Drawing.Point(-1, 21);
            this.mBack.Name = "mBack";
            this.mBack.Size = new System.Drawing.Size(42, 36);
            this.mBack.TabIndex = 1;
            this.mBack.UseSelectable = true;
            this.mBack.Click += new System.EventHandler(this.mBack_Click);
            // 
            // trayNotifier
            // 
            this.trayNotifier.ContextMenuStrip = this.contextMenuStrip1;
            this.trayNotifier.Icon = ((System.Drawing.Icon)(resources.GetObject("trayNotifier.Icon")));
            this.trayNotifier.Text = "TrayNotifier";
            this.trayNotifier.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(99, 48);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.startToolStripMenuItem.Text = "Start";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // ChatForm
            // 
            this.AcceptButton = this.mSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 530);
            this.Controls.Add(this.mSend);
            this.Controls.Add(this.rchtxtChat);
            this.Controls.Add(this.rchTxtChatShow);
            this.Controls.Add(this.mBack);
            this.Name = "ChatForm";
            this.Text = "   HI Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroLink mBack;
        private System.Windows.Forms.RichTextBox rchTxtChatShow;
        private System.Windows.Forms.RichTextBox rchtxtChat;
        private MetroFramework.Controls.MetroLink mSend;
        private System.Windows.Forms.NotifyIcon trayNotifier;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}