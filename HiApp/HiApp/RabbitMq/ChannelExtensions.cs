using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Windows.Forms;

namespace HiApp.RabbitMq
{
    public static class ChannelExtensions
    {
        /// <summary>
        /// StartConsume - method is use to start cosume the consumer.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="queueName"></param>
        /// <param name="callback"></param>
        public static void StartConsume(this IModel channel, string queueName, Action<IModel, DefaultBasicConsumer, BasicDeliverEventArgs> callback)
        {
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(queueName, true, consumer);
            while (true)
            {
                try
                {
                    var eventArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    callback(channel, consumer, eventArgs);
                }
                catch (EndOfStreamException)
                {
                    // The consumer was cancelled, the model closed, or the connection went away.
                    break;
                }
            }
        }

        /// <summary>
        /// use to invoke the specific action based on control.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        public static void InvokeIfRequired(this Control control, MethodInvoker action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }
        /// <summary>
        /// ScrollToEnd - limit text box scroll
        /// </summary>
        /// <param name="textbox"></param>
        public static void ScrollToEnd(this RichTextBox textbox)
        {
            textbox.Select(textbox.Text.Length - 1, 0);
            textbox.ScrollToCaret();
        }
    }
}
