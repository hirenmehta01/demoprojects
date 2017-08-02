using System;
using MetroFramework.Forms;

namespace HiApp
{
    public partial class Welcome : MetroForm
    {
        

        public Welcome()
        {
            InitializeComponent();
        }

        private void mchat_Click(object sender, EventArgs e)
        {
            ChatForm chatFrm = new ChatForm();
            chatFrm.Show();
            this.Hide();
        }
    }
}
