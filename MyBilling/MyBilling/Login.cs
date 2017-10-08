using MyBilling.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyBilling.DAL;
using MyBilling.Helper;
using System.IO;
namespace MyBilling
{
    public partial class Login : Form
    {
        public Login()
        {
            this.KeyDown += new KeyEventHandler(form_KeyDown);
            InitializeComponent();
        }

        /// <summary>
        /// form_KeyPress
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void form_KeyDown(object o, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
                GetLogin();
            else if (e.KeyCode.Equals(Keys.Escape))
                FormClose();
        }

        /// <summary>
        /// ok button click which is required use name and password for loginn and proceed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            GetLogin();
        }

        /// <summary>
        /// GetLogin - required for login
        /// </summary>
        private void GetLogin()
        {
            try
            {
                if (string.IsNullOrEmpty(txtusername.Text) || string.IsNullOrEmpty(txtpassword.Text))
                {
                    MessageBox.Show("Details Required", "Login Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading, false);
                    if (string.IsNullOrEmpty(txtusername.Text)) txtusername.Focus();
                    else txtpassword.Focus();
                    return;
                }
                //MessageBox.Show(BillingHelper.DbPath);
                //database process start for check user existance.
                using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
                {
                    string loginQuery = "select * from user where userid= '{0}' and password= '{1}'";
                    var loginData = context.Select(string.Format(loginQuery, txtusername.Text, txtpassword.Text));
                    if (loginData.Rows.Count.Equals(0))
                    {
                        MessageBox.Show("Invalid detail", "Login Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading, false);
                        txtusername.Focus();
                        return;
                        
                    }
                    BillingHelper.UserId = loginData.Rows[0]["userid"].ToString();
                    BillingHelper.UserType = Convert.ToInt16(loginData.Rows[0]["usertypeid"]);
                };
                Program.SetMainForm(new Home());
                Program.ShowMainForm();
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this button is useful for close the app.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private static void FormClose()
        {
            CancelEventArgs cancelEvent = new CancelEventArgs(true);
            Application.Exit(cancelEvent);
        }

        /// <summary>
        /// Login_Load - reuqired for set focus at user event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtusername;
        }
    }
}
