using CrystalDecisions.CrystalReports.Engine;
using MyBilling.DAL;
using MyBilling.Entities;
using MyBilling.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace MyBilling
{
    public partial class Home : Form
    {
        /// <summary>
        /// CalculateAmount - for calculate amount
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        private delegate string CalculateAmount(string rate, string qty);
        string GlobalMode = "0"; // This is the Mode to Passed as Parameter to SaveData function
        string RegGlobalMode = "0"; // This is the Mode to Passed as Parameter to SaveData function
        int id { get; set; }

        int CompanyId { get; set; }

        long MaxBillNo { get; set; }
        private int WindowHeight { get { return Screen.PrimaryScreen.WorkingArea.Height; } }
        private int WindowWidth { get { return Screen.PrimaryScreen.WorkingArea.Width; } }

        private static Company CompanyInfo { get; set; }
        public Home()
        {
            InitializeComponent();
            FetchCompanyDetails();
            txtrate.TextChanged += new EventHandler(SetAmount);
            txtTransRate.TextChanged += new EventHandler(SetTransAmount);
            txtquantity.TextChanged += new EventHandler(SetAmount);
            txtquantity.TextChanged += new EventHandler(SetTransAmount);
            tabControl.SelectedIndexChanged += new EventHandler(tabControl_SelectedIndexChanged);
            txtrate.KeyPress += new KeyPressEventHandler(textbox_KeyPress);
            txtquantity.KeyPress += new KeyPressEventHandler(textbox_KeyPress);
            txtVAT.KeyPress += textbox_KeyPress;
            rdlTransportaion.CheckedChanged += CheckChangeEvent;
            rdlMaterial.CheckedChanged += CheckChangeEvent;
            txtTransRate.KeyPress += textbox_KeyPress;
            dtpDate.Format = DateTimePickerFormat.Custom;
            dtpDate.CustomFormat = "dd/MM/yyyy";
            dpbillingfrom.Format = DateTimePickerFormat.Custom;
            dpbillingfrom.CustomFormat = "dd/MM/yyyy";
            dpBillingTo.Format = DateTimePickerFormat.Custom;
            dpBillingTo.CustomFormat = "dd/MM/yyyy";
            dtpDate.Value = DateTime.Now;
            dpbillingfrom.Value = DateTime.Now;
            dpBillingTo.Value = DateTime.Now;
            BindRegCompanyGrid();
            BindRegCompanyCombo();
            BindGrid();
            rdlMaterial.Checked = true;
            tabControl.ClientSize = new Size(WindowWidth, WindowHeight);
            crvReportResult.ClientSize = new Size(WindowWidth, WindowHeight);
            if (CompanyInfo == null)
            {
                tabControl.SelectTab("tabCompanyDetail");
            }
        }

        /// <summary>
        /// tabControl_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CompanyInfo == null)
                tabControl.SelectTab("tabCompanyDetail");
            BindRegCompanyCombo();
            if (tabControl.SelectedTab.Name.Equals("tbReport"))
                txtBillNo.Text = SetMaxBillNo().ToString();
        }

        private void BillingEntry_Load(object sender, EventArgs e)
        {
            this.crvReportResult.RefreshReport();
        }

        private void BindGrid()
        {
            //database process start for check user existance.
            using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
            {
                context.OpenConnection();
                string queryForGetGridData = cmbRegComp.SelectedValue.ToString().Equals("0") ?
                    "select edt.vehicleid as VehicleNo,edt.quantity as Quantity,rc.Name,edt.rate as Rate,edt.amount As Amount,edt.itemname as MaterialName,date(en.date) as Date,en.entryid as EntryId,edt.transportation Transportaion,edt.transportationcost TransportaionCost from entry en join entrydetail edt on en.entryid = edt.entryid join registercompany rc on en.regcompanyid = rc.companyid where date(en.date) >= date('" + dtpDate.Value.ToString("s") + "') "
                    : "select edt.vehicleid as VehicleNo,edt.quantity as Quantity,rc.Name,edt.rate as Rate,edt.amount As Amount,edt.itemname as ItemName,date(en.date) as Date,en.entryid as EntryId,edt.transportation Transportaion,edt.transportationcost TransportaionCost from entry en join entrydetail edt on en.entryid = edt.entryid join registercompany rc on en.regcompanyid = rc.companyid where date(en.date) >= date('" + dtpDate.Value.ToString("s") + "') and en.regcompanyid = " + cmbRegComp.SelectedValue;
                var gridData = context.Select(queryForGetGridData);
                dgItemData.DataSource = gridData;
            };
        }

        private void BindRegCompanyGrid()
        {
            //database process start for check user existance.
            using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
            {
                context.OpenConnection();
                string queryForGetGridData = "select Name,Add1,Add2,Add3,City,CompanyId from registercompany ";
                var gridData = context.Select(queryForGetGridData);
                dgRegCompany.DataSource = gridData;
            };
        }

        private void BindRegCompanyCombo()
        {
            //database process start for check user existance.
            using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
            {
                DataRow dr;
                context.OpenConnection();
                string queryForGetGridData = "select Name,CompanyId from registercompany ";
                var gridData = context.Select(queryForGetGridData);
                dr = gridData.NewRow();
                dr.ItemArray = new object[] { "--Select BillingTo--", 0 };
                gridData.Rows.InsertAt(dr, 0);
                cmbRegComp.DataSource = new BindingSource(gridData, null);
                cmbRegComp.DisplayMember = "Name";
                cmbRegComp.ValueMember = "CompanyId";
                cmbBillingToname.DataSource = new BindingSource(gridData, null);
                cmbBillingToname.DisplayMember = "Name";
                cmbBillingToname.ValueMember = "CompanyId";
            };
        }

        // This event sets the mode for edit i.e. GlobalMode = 1 and also deletes the data by identifying the column name
        private void dgItemData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgItemData.Columns[e.ColumnIndex].Name == "Edit")
            {
                id = Convert.ToInt32(dgItemData.Rows[e.RowIndex].Cells["entryid"].Value);
                GetDataforUpdate(getdatatoTextbox(id));
                btnbsave.Text = "UPDATE";
                GlobalMode = "1";
            }
            else if (dgItemData.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result1 = MessageBox.Show("Are you sure you want to delete\nrecord of " + dgItemData.Rows[e.RowIndex].Cells["itemname"].Value.ToString() + " ?", "Warning", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    //database process start for check user existance.
                    using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
                    {
                        context.OpenConnection();
                        string deleteItemDetailQuery = "DELETE FROM entrydetail WHERE entryid =" + Convert.ToInt32(dgItemData.Rows[e.RowIndex].Cells["entryid"].Value);
                        context.Execute(deleteItemDetailQuery);
                        string deleteItemQuery = "DELETE FROM entry WHERE entryid =" + Convert.ToInt32(dgItemData.Rows[e.RowIndex].Cells["entryid"].Value);
                        context.Execute(deleteItemQuery);
                        MessageBox.Show("Entry Deleted!", "My Billing - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        BindGrid();
                    };
                }
            }
        }

        // This event sets the mode for edit i.e. GlobalMode = 1 and also deletes the data by identifying the column name
        private void dgRegCompany_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgRegCompany.Columns[e.ColumnIndex].Name == "RegEdit")
            {
                CompanyId = Convert.ToInt32(dgRegCompany.Rows[e.RowIndex].Cells["CompanyId"].Value);
                GetRegCompDataforUpdate(getregcompanydatatoTextbox(CompanyId));
                btnRegSave.Text = "UPDATE";
                RegGlobalMode = "1";
            }
            else if (dgRegCompany.Columns[e.ColumnIndex].Name == "RegDelete")
            {
                DialogResult result1 = MessageBox.Show("Are you sure you want to delete\nrecord of " + dgRegCompany.Rows[e.RowIndex].Cells["Name"].Value.ToString() + " ?", "Warning", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    //database process start for check user existance.
                    using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
                    {
                        context.OpenConnection();
                        string deleteItemDetailQuery = "DELETE FROM registercompany WHERE CompanyId =" + Convert.ToInt32(dgRegCompany.Rows[e.RowIndex].Cells["CompanyId"].Value);
                        context.Execute(deleteItemDetailQuery);
                        MessageBox.Show("Entry Deleted!", "My Billing - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        BindRegCompanyGrid();
                    };
                }
            }
        }

        // This function actually passes the data to textbox
        private DataTable getdatatoTextbox(int id)
        {
            //database process start for check user existance.
            using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
            {
                context.OpenConnection();
                string queryForGetGridData = "select edt.transportation Transportaion,edt.transportationcost TransportaionCost,edt.vehicleid as VehicleNo,edt.quantity as Quantity,edt.rate as Rate,edt.amount As Amount,edt.itemname as ItemName,en.date as Date,en.entryid as EntryId,en.regcompanyid as RegCompanyId from entry en join entrydetail edt on en.entryid = edt.entryid where en.entryid = " + id;
                var gridData = context.Select(queryForGetGridData);
                return gridData;
            };
        }

        // This function actually passes the data to textbox
        private DataTable getregcompanydatatoTextbox(int id)
        {
            //database process start for check user existance.
            using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
            {
                context.OpenConnection();
                string queryForGetGridData = "select Name,Add1,Add2,Add3,City from registercompany where companyid = " + id;
                var gridData = context.Select(queryForGetGridData);
                return gridData;
            };
        }

        // This function gets the data into datatable 
        private void GetDataforUpdate(DataTable dtdata)
        {
            dtpDate.Value = Convert.ToDateTime(dtdata.Rows[0]["date"]);
            txtrate.Text = dtdata.Rows[0]["rate"].ToString();
            txtquantity.Text = dtdata.Rows[0]["quantity"].ToString();
            txtamount.Text = dtdata.Rows[0]["amount"].ToString();
            txtitemname.Text = dtdata.Rows[0]["itemname"].ToString();
            txtvehicleno.Text = dtdata.Rows[0]["vehicleno"].ToString();
            txtTransRate.Text = dtdata.Rows[0]["Transportaion"].ToString();
            txtTransCost.Text = dtdata.Rows[0]["TransportaionCost"].ToString();
            cmbRegComp.SelectedValue = dtdata.Rows[0]["RegCompanyId"].ToString();
        }

        // This function gets the data into datatable 
        private void GetRegCompDataforUpdate(DataTable dtdata)
        {
            txtRegCompName.Text = dtdata.Rows[0]["Name"].ToString();
            txtRegAdd1.Text = dtdata.Rows[0]["Add1"].ToString();
            txtRegAdd2.Text = dtdata.Rows[0]["Add2"].ToString();
            txtRegAdd3.Text = dtdata.Rows[0]["Add3"].ToString();
            txtRegCity.Text = dtdata.Rows[0]["City"].ToString();
        }
        /// <summary>
        /// CheckChangeEvent - which is called on radiobuttion chnage evnet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckChangeEvent(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Name.Equals("rdlTransportaion"))
            {
                txtVAT.Text = "0";
                txtVAT.Enabled = false;
            }
            else
            {
                txtVAT.Text = "";
                txtVAT.Enabled = true;
            }
        }

        private void btnbsave_Click(object sender, EventArgs e)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (string.IsNullOrEmpty(txtitemname.Text) || string.IsNullOrEmpty(txtvehicleno.Text) || cmbRegComp.SelectedValue.ToString().Equals("0"))
                    {
                        MessageBox.Show("Details Required", "Login Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading, false);
                        if (string.IsNullOrEmpty(txtitemname.Text)) txtitemname.Focus();
                        else txtvehicleno.Focus();
                        return;
                    }
                    //database process start for check user existance.
                    using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
                    {
                        context.OpenConnection();
                        if (GlobalMode.Equals("1"))
                        {
                            string entryDetailTableUpdateQuery = "update entrydetail set itemname = '" + txtitemname.Text + "', vehicleid = '" + txtvehicleno.Text + "',rate = '" + txtrate.Text + "', quantity = '" + txtquantity.Text + "', amount = '" + txtamount.Text + "',transportationcost = '" + txtTransCost.Text + "',transportation = '" + txtTransRate.Text + "' where entryid = " + id;
                            context.Execute(entryDetailTableUpdateQuery);
                            string entryTableUpdateQuery = "update entry set date = '" + dtpDate.Value.ToString("s") + "',regcompanyid = '" + cmbRegComp.SelectedValue + "' where entryid = " + id;
                            context.Execute(entryTableUpdateQuery);
                            GlobalMode = "0";

                        }
                        else
                        {
                            string queryForMaxId = "select ifnull(max(entryid),0) from entry";
                            long msxEntryId = context.ExecuteScalar<long>(queryForMaxId);
                            long entryId = msxEntryId + 1;
                            string entryTableInsertQuery = "INSERT INTO [entry]([entryid],[date],[userid],regcompanyid)VALUES(" + entryId + ",'" + dtpDate.Value.ToString("s") + "','" + BillingHelper.UserId + "','" + cmbRegComp.SelectedValue + "')";
                            context.Execute(entryTableInsertQuery);
                            string entryDetailTableInsertQuery = "INSERT INTO [entrydetail]([entryid],[vehicleid],[itemname],[rate],[amount],[quantity],[transportation],[transportationcost])VALUES(" + entryId + ",'" + txtvehicleno.Text + "','" + txtitemname.Text + "','" + txtrate.Text + "','" + txtamount.Text + "','" + txtquantity.Text + "','" + txtTransRate.Text + "','" + txtTransCost.Text + "')";
                            context.Execute(entryDetailTableInsertQuery);
                        }

                        MessageBox.Show("Entry saved!", "My Billing - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    };
                    scope.Complete();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message, "My Billing - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { scope.Dispose(); }
                ClearControls();
                BindGrid();
            }
        }

        /// <summary>
        /// SetMaxBillNo
        /// </summary>
        private long SetMaxBillNo()
        {
            //database process start for check user existance.
            using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
            {
                context.OpenConnection();
                string queryForMaxId = "select ifnull(max(billno),0) from billno";
                long msxEntryId = context.ExecuteScalar<long>(queryForMaxId);
                MaxBillNo = msxEntryId + 1;
                return MaxBillNo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveMaxBillNo()
        {
            //database process start for check user existance.
            using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
            {
                context.OpenConnection();
                string entryTableInsertQuery = "INSERT INTO [billno]([billno])VALUES(" + txtBillNo.Text + ")";
                context.Execute(entryTableInsertQuery);
            }
        }
        private void btnRegSave_Click(object sender, EventArgs e)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (string.IsNullOrEmpty(txtRegCompName.Text))
                    {
                        MessageBox.Show("Company Name Required.", "Details required", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading, false);
                        txtRegCompName.Focus();
                        return;
                    }
                    //database process start for check user existance.
                    using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
                    {
                        context.OpenConnection();
                        if (RegGlobalMode.Equals("1"))
                        {
                            string entryDetailTableUpdateQuery = "update registercompany set Name = '" + txtRegCompName.Text + "', Add1 = '" + txtRegAdd1.Text + "',Add2 = '" + txtRegAdd2.Text + "', Add3 = '" + txtRegAdd3.Text + "', City = '" + txtRegCity.Text + "' where CompanyId = " + CompanyId;
                            context.Execute(entryDetailTableUpdateQuery);
                            RegGlobalMode = "0";
                        }
                        else
                        {
                            string companyTableInsertQuery = "INSERT INTO [registercompany]([Name],[Add1],[Add2],[Add3],[City])VALUES('" + txtRegCompName.Text + "','" + txtRegAdd1.Text + "','" + txtRegAdd2.Text + "','" + txtRegAdd3.Text + "','" + txtRegCity.Text + "')";
                            context.Execute(companyTableInsertQuery);
                        }
                        MessageBox.Show("Entry saved!", "My Billing - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    };
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message, "My Billing - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { scope.Dispose(); }
                BindRegCompanyGrid();
                ClearRegComapnyControls();
            }
        }


        /// <summary>
        /// ClearControl - clear all controls.
        /// </summary>
        private void ClearControls()
        {
            tabsales.Controls.ClearControls<TextBox>();
            tabsales.Controls.ClearControls<DateTimePicker>();
            cmbRegComp.SelectedValue = 0;
            dtpDate.Value = DateTime.Now;
            btnbsave.Text = "Save";
        }

        /// <summary>
        /// ClearControl - clear all controls.
        /// </summary>
        private void ClearRegComapnyControls()
        {
            tabRegCompany.Controls.ClearControls<TextBox>();
            btnbsave.Text = "Save";
        }

        /// <summary>
        /// SetAmount - based on rate and qty
        /// </summary>
        private void SetAmount(object sender, EventArgs args)
        {
            CalculateAmount calcDel = new CalculateAmount(Calculate);
            txtamount.Text = calcDel.Invoke(txtrate.Text, txtquantity.Text);
        }
        /// <summary>
        /// SetAmount - based on rate and qty
        /// </summary>
        private void SetTransAmount(object sender, EventArgs args)
        {
            CalculateAmount calcDel = new CalculateAmount(Calculate);
            txtTransCost.Text = calcDel.Invoke(txtTransRate.Text, txtquantity.Text);
        }
        /// <summary>
        /// Calculate - calc amount based on rate*qty
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        private string Calculate(string rate, string qty)
        {
            if (string.IsNullOrEmpty(qty) || string.IsNullOrEmpty(rate)) return string.Empty;
            else return (Convert.ToDecimal(qty) * Convert.ToDecimal(rate)).ToString();
        }

        /// <summary>
        /// show report - based on date selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtVAT.Text) || (cmbBillingToname.SelectedValue.ToString().Equals("0")))
                {
                    MessageBox.Show("Please enter all required details.", "Required Fields!", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    return;
                }
                if (dpbillingfrom.Value > dpBillingTo.Value)
                {
                    MessageBox.Show("Billing from date should not greater than billing to date.", "Date validation!", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    return;
                }
                var reportDataSource = GetData();
                if (reportDataSource.Rows.Count == 0)
                {
                    MessageBox.Show("No data found!", "Execution", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    return;
                }
                ReportDocument cryRpt = new ReportDocument();
                cryRpt.Load(BillingHelper.ReportPath);
                cryRpt.SetDataSource(reportDataSource);
                SetReportTextData(cryRpt);
                cryRpt.SetParameterValue("VatPercent", txtVAT.Text);
                tabControl.SelectTab("tabReportData");
                crvReportResult.ReportSource = cryRpt;
                crvReportResult.Refresh();
                SaveMaxBillNo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message, "Error");
            }


        }

        /// <summary>
        /// SetReportTextData - set report Text Data.
        /// </summary>
        /// <param name="cryRpt"></param>
        private void SetReportTextData(ReportDocument cryRpt)
        {
            //find objects from report sections.
            TextObject txtCompanyName = (TextObject)cryRpt.ReportDefinition.Sections["Section1"].ReportObjects["txtCompanyName"];
            TextObject txtAdd1 = (TextObject)cryRpt.ReportDefinition.Sections["Section1"].ReportObjects["txtAdd1"];
            TextObject txtAdd2 = (TextObject)cryRpt.ReportDefinition.Sections["Section1"].ReportObjects["txtAdd2"];
            TextObject txtAdd3 = (TextObject)cryRpt.ReportDefinition.Sections["Section1"].ReportObjects["txtAdd3"];
            TextObject txtBuilder = (TextObject)cryRpt.ReportDefinition.Sections["Section1"].ReportObjects["txtBuilderName"];
            TextObject txtCity = (TextObject)cryRpt.ReportDefinition.Sections["Section1"].ReportObjects["txtCity"];
            TextObject txtBilingFrom = (TextObject)cryRpt.ReportDefinition.Sections["Section2"].ReportObjects["txtBilingFrom"];
            TextObject txtRptBillNo = (TextObject)cryRpt.ReportDefinition.Sections["Section2"].ReportObjects["txtRptBillNo"];
            TextObject txtBilingTo = (TextObject)cryRpt.ReportDefinition.Sections["Section2"].ReportObjects["txtBillingTo"];
            TextObject txtRateHeader = (TextObject)cryRpt.ReportDefinition.Sections["Section2"].ReportObjects["txtRateHeader"];
            TextObject txtAmountHeader = (TextObject)cryRpt.ReportDefinition.Sections["Section2"].ReportObjects["txtAmountHeader"];
            TextObject txtVatNumber = (TextObject)cryRpt.ReportDefinition.Sections["Section5"].ReportObjects["txtVatNumber"];
            TextObject txtFooterCompany = (TextObject)cryRpt.ReportDefinition.Sections["Section5"].ReportObjects["txtFooterCompany"];

            txtRptBillNo.Text = MaxBillNo.ToString();
            //fetch biiling to name...
            if (!cmbBillingToname.SelectedValue.Equals(0))
            {
                //database process start for check user existance.
                using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
                {
                    context.OpenConnection();
                    string companyQuery = "select City,Add1,Add2,Add3,City,Name from registercompany where CompanyId = " + cmbBillingToname.SelectedValue;
                    var regCompData = context.Select(companyQuery);
                    txtBuilder.Text = regCompData.Rows[0]["Name"].ToString();
                    StringBuilder sbToAdd = new StringBuilder();
                    string Add1 = Convert.ToString(regCompData.Rows[0]["Add1"]);
                    string Add2 = Convert.ToString(regCompData.Rows[0]["Add2"]);
                    string Add3 = Convert.ToString(regCompData.Rows[0]["Add3"]);
                    string City = Convert.ToString(regCompData.Rows[0]["City"]);
                    if (!String.IsNullOrEmpty(Add1))
                        sbToAdd.AppendLine(Add1 + ",");
                    if (!string.IsNullOrEmpty(Add2))
                        sbToAdd.AppendLine(Add2 + ",");
                    if (!string.IsNullOrEmpty(Add3))
                        sbToAdd.Append(Add3 + ",");
                    if (!string.IsNullOrEmpty(City))
                        sbToAdd.Append(City);
                    txtCity.Text = sbToAdd.ToString();
                }
            }

            //set values
            txtCompanyName.Text = CompanyInfo.CompanyName;
            txtAdd1.Text = CompanyInfo.Address1;
            txtAdd2.Text = CompanyInfo.Address2;
            txtAdd3.Text = CompanyInfo.Address3;
            txtBilingFrom.Text = dpbillingfrom.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            txtBilingTo.Text = dpBillingTo.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            txtVatNumber.Text = CompanyInfo.VATTinNo;
            txtFooterCompany.Text = "For " + CompanyInfo.CompanyName;
            if (rdlMaterial.Checked)
            {
                txtRateHeader.Text = "Rate/Brass";
                txtAmountHeader.Text = "Amount";
            }
            else
            {
                txtRateHeader.Text = "Trans.Rate";
                txtAmountHeader.Text = "Trans.Cost";
            }
        }

        /// <summary>
        /// GetReport - get source for report
        /// </summary>
        /// <returns></returns>
        private DataTable GetData()
        {
            //database process start for check user existance.
            using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
            {
                context.OpenConnection();
                string reportQuery = "";
                if (rdlMaterial.Checked)
                {
                    reportQuery = cmbBillingToname.SelectedValue.Equals(0) ?
                       "select edt.vehicleid as VehicleNo,edt.quantity as Quantity,edt.rate As Rate,edt.amount as Amount,edt.itemname As ItemName,strftime('%d-%m-%Y', date(en.date)) As Date from entry en join entrydetail edt on en.entryid = edt.entryid where date(en.date) >= date('" + dpbillingfrom.Value.ToString("s") + "') and date(en.date) <= date('" + dpBillingTo.Value.ToString("s") + "') ORDER BY en.date DESC " :
                       "select edt.vehicleid as VehicleNo,edt.quantity as Quantity,edt.rate As Rate,edt.amount as Amount,edt.itemname As ItemName,strftime('%d-%m-%Y', date(en.date)) As Date from entry en join entrydetail edt on en.entryid = edt.entryid where date(en.date) >= date('" + dpbillingfrom.Value.ToString("s") + "') and date(en.date) <= date('" + dpBillingTo.Value.ToString("s") + "') and en.regcompanyid = " + cmbBillingToname.SelectedValue + " ORDER BY en.date DESC ";
                }
                else
                {
                    reportQuery = cmbBillingToname.SelectedValue.Equals(0) ?
                       "select edt.vehicleid as VehicleNo,edt.quantity as Quantity,edt.transportation As Rate,edt.transportationcost as Amount,edt.itemname As ItemName,strftime('%d-%m-%Y', date(en.date)) As Date from entry en join entrydetail edt on en.entryid = edt.entryid where date(en.date) >= date('" + dpbillingfrom.Value.ToString("s") + "') and date(en.date) <= date('" + dpBillingTo.Value.ToString("s") + "') ORDER BY en.date DESC" :
                       "select edt.vehicleid as VehicleNo,edt.quantity as Quantity,edt.transportation As Rate,edt.transportationcost as Amount,edt.itemname As ItemName,strftime('%d-%m-%Y', date(en.date)) As Date from entry en join entrydetail edt on en.entryid = edt.entryid where date(en.date) >= date('" + dpbillingfrom.Value.ToString("s") + "') and date(en.date) <= date('" + dpBillingTo.Value.ToString("s") + "') and en.regcompanyid = " + cmbBillingToname.SelectedValue + " ORDER BY en.date DESC ";
                }
                var reportData = context.Select(reportQuery);
                return reportData;
            };
        }

        /// <summary>
        /// btnCompanySave_Click = use for save company details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCompanySave_Click(object sender, EventArgs e)
        {
            //database process start for check user existance.
            using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
            {
                context.OpenConnection();
                string companyTableInsertQuery = "INSERT INTO [companydetail]([Name],[Add1],[Add2],[Add3],[City],[vattinno])VALUES('" + txtCompanyName.Text + "','" + txtAdd1.Text + "','" + txtAdd2.Text + "','" + txtAddress3.Text + "','" + txtCity.Text + "','" + txtVatNo.Text + "')";
                if (CompanyInfo != null)
                {
                    companyTableInsertQuery = "update [companydetail] SET Name = '" + txtCompanyName.Text + "',City = '" + txtCity.Text + "' , Add1 = '" + txtAdd1.Text + "' , Add2 = '" + txtAdd2.Text + "', Add3 = '" + txtAddress3.Text + "', vattinno = '" + txtVatNo.Text + "' WHERE CompanyId = " + CompanyInfo.CompanyId;
                }
                context.Execute(companyTableInsertQuery);
                MessageBox.Show("Company details saved!", "My Billing - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FetchCompanyDetails();
            };
        }

        /// <summary>
        /// FetchCoequirmpanyDetails - fetch Company details based on action r
        /// </summary>
        private void FetchCompanyDetails()
        {
            //database process start for check user existance.
            using (SQLiteHelper context = new SQLiteHelper(BillingHelper.DbPath))
            {
                string companyFetchQuery = "select * from companydetail";
                var companyData = context.Select(companyFetchQuery);
                if (companyData.Rows.Count.Equals(0))
                    return;
                CompanyInfo = new Company();
                CompanyInfo.CompanyId = Convert.ToInt16(companyData.Rows[0]["CompanyId"]);
                CompanyInfo.CompanyName = companyData.Rows[0]["Name"].ToString();
                CompanyInfo.City = Convert.ToString(companyData.Rows[0]["City"]);
                CompanyInfo.Address1 = Convert.ToString(companyData.Rows[0]["Add1"]);
                CompanyInfo.Address2 = Convert.ToString(companyData.Rows[0]["Add2"]);
                CompanyInfo.Address3 = Convert.ToString(companyData.Rows[0]["Add3"]);
                CompanyInfo.VATTinNo = Convert.ToString(companyData.Rows[0]["vattinno"]);
                txtCompanyName.Text = CompanyInfo.CompanyName;
                txtCity.Text = CompanyInfo.City;
                txtAdd1.Text = CompanyInfo.Address1;
                txtAdd2.Text = CompanyInfo.Address2;
                txtAddress3.Text = CompanyInfo.Address3;
            }
        }

        private void btnViewGrid_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// ClearControls - clear controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnbnew_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        /// <summary>
        /// RegEX - For Enter Only Num
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string groupSeparator = numberFormatInfo.NumberGroupSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();

            if (Char.IsDigit(e.KeyChar))
            {
                // Digits are OK
            }
            else if (keyInput.Equals(decimalSeparator) || keyInput.Equals(groupSeparator) ||
             keyInput.Equals(negativeSign))
            {
                // Decimal separator is OK
            }
            else if (e.KeyChar == '\b')
            {
                // Backspace key is OK
            }
            else
            {
                // Swallow this invalid key and beep
                e.Handled = true;
                //    MessageBeep();
            }
        }

        /// <summary>
        /// exitToolStripMenuItem1_Click - For Logout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Program.SetMainForm(new Login());
            Program.ShowMainForm();
            this.Close();
        }

        private void reportsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tabControl.SelectTab("tbReport");
        }

        private void billingEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl.SelectTab("tabsales");
        }

        private void companyDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl.SelectTab("tabCompanyDetail");
        }

        private void radioChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdlTransportaion_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
