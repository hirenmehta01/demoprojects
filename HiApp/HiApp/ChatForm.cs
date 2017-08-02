using System;
using System.Threading.Tasks;
using MetroFramework.Forms;
using RabbitMQ.Client;
using HiApp.RabbitMq;
using RabbitMQ.Client.Events;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace HiApp
{
    public partial class ChatForm : MetroForm
    {
        MQConnection mqConnection = new MQConnection();
        IModel channelSend = null;
        IModel channelReceive = null;
        Task receivingTask = null;
        string userId = "Hiren_Mehta01_" + DateTime.Now.ToString("ddMMyyhhmmss");

        public ChatForm()
        {
            InitializeComponent();
            rchTxtChatShow.Text = "Do not share any private details unless the person is quite convincing or insistent.\r\n";
            mqConnection.Connect();
            channelSend = mqConnection.connection.CreateModel();
            channelSend.ExchangeDeclare(MQConnection.ExchangeName, ExchangeType.Fanout, false, true, null);
            channelReceive = mqConnection.connection.CreateModel();
            channelReceive.QueueDeclare(userId, false, false, true, null);
            channelReceive.QueueBind(userId, MQConnection.ExchangeName, "");
            receivingTask = new Task(() => channelReceive.StartConsume(userId, MessageHandler));
            //receivingTask.Name = "ReceivingThread"; //name the thread so that when it goes insane you will be able to apportion blame.
            receivingTask.Start();
            rchtxtChat.Focus();
        }
        /// <summary>
        /// delegate performing the message appending and callback
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="consumer"></param>
        /// <param name="eventArgs"></param>
        public void MessageHandler(IModel channel, DefaultBasicConsumer consumer, BasicDeliverEventArgs eventArgs)
        {
            string message = Encoding.UTF8.GetString(eventArgs.Body) + "\r\n";
            rchTxtChatShow.InvokeIfRequired(() =>
            {
                rchTxtChatShow.Text += message;
                rchTxtChatShow.ScrollToEnd();
            });
            Screen_Move(message);
            // Flash window 5 times
            //FlashWindow.Flash(this, 5);
        }

        /// <summary>
        /// use for show bubble when screen is minimize
        /// </summary>
        /// <param name="message"></param>
        private void Screen_Move(string message)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                trayNotifier.ShowBalloonTip(1000, "Important Notice", message, ToolTipIcon.Info);
            }
        }

        private void mBack_Click(object sender, EventArgs e)
        {
            Welcome wlcFrm = new Welcome();
            this.Hide();
            wlcFrm.Show();
        }
        /// <summary>
        /// after close the form wipe out everything from channel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (channelSend.IsOpen) channelSend.Close();
            if (channelReceive.IsOpen) channelReceive.Close();
            if (mqConnection.connection.IsOpen) mqConnection.connection.Close();
        }

        private void ChatForm_ResizeEnd(object sender, EventArgs e)
        {
            rchTxtChatShow.ScrollToEnd();
        }

        private void trayNotifier_MouseClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void trayNotifier_BalloonTipClicked(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void trayNotifier_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void mSend_Click(object sender, EventArgs e)
        {
            string input = " " + userId + " > " + rchtxtChat.Text;
            byte[] message = Encoding.UTF8.GetBytes(input);
            channelSend.BasicPublish(MQConnection.ExchangeName, "", null, message);
            rchtxtChat.Text = string.Empty;
            rchtxtChat.Focus();
        }
    }
}
