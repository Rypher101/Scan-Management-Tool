using IBM_Scan_Manager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static IBM_Scan_Manager.Classes.FindingStatus;

namespace IBM_Scan_Manager.Forms
{
    public partial class frmViewFindings : Form
    {
        private frmDGV DGVForm = null;
        private List<TblAssessment> findings;
        private List<TblAssessment> filteredFindings;
        private int scanID;
        public TblAssessment selectedItem;
        private bool useFiltered = false, isFromSetValue = false;
        private List<string> sortOrder = new List<string>();
        public frmViewFindings(int scanID = 10)
        {
            this.scanID = scanID;
            InitializeComponent();
        }

        private void frmViewFindings_Load(object sender, EventArgs e)
        {
            using (var context = new IBMScanDBContext())
            {
                findings = context.TblAssessments.Where(e => e.ScanId == scanID).ToList();
                filteredFindings = findings;

                if (findings == null || findings.Count < 1)
                {
                    MessageBox.Show("Cannot find scan details related to scan ID : " + scanID.ToString() + Environment.NewLine + "Pleae try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }

                SetValues(findings.First());
                SetAutoCompleteTxtBox();
            }

            cmbExcel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus2.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbExcel.SelectedIndex = 0;
            cmbStatus.DataSource = Enum.GetValues(typeof(Status));
            cmbStatus2.DataSource = Enum.GetValues(typeof(Status));
        }

        private void SetAutoCompleteTxtBox()
        {
            AutoCompleteStringCollection acClassification = new AutoCompleteStringCollection();
            AutoCompleteStringCollection acVulnerability = new AutoCompleteStringCollection();
            AutoCompleteStringCollection acAPI = new AutoCompleteStringCollection();
            AutoCompleteStringCollection acContext = new AutoCompleteStringCollection();
            AutoCompleteStringCollection acSource = new AutoCompleteStringCollection();
            AutoCompleteStringCollection acComment = new AutoCompleteStringCollection();

            foreach (var item in findings)
            {
                acClassification.Add(item.Classification);
                acVulnerability.Add(item.Vulnerability);
                acAPI.Add(item.Api);
                acContext.Add(item.Context);
                acSource.Add(item.SourceFile.Split(@"\").Last());
                acComment.Add(item.Comment);
            }

            txtClassification.AutoCompleteCustomSource = acClassification;
            txtAPI.AutoCompleteCustomSource = acAPI;
            txtComment2.AutoCompleteCustomSource = acComment;
            txtContext.AutoCompleteCustomSource = acContext;
            txtSource.AutoCompleteCustomSource = acSource;
            txtVulnerabiluty.AutoCompleteCustomSource = acVulnerability;
        }

        private void SetValues(TblAssessment model)
        {
            isFromSetValue = true;
            selectedItem = model;
            lblClassification.Text = model.Classification;
            lblContext.Text = model.Context;
            lblExcel.Text = model.InExcel ? "Yes" : "No";
            lblLine.Text = model.LineNum.ToString();
            lblAPI.Text = model.Api;
            lblSource.Text = model.SourceFile;
            lblVulnerability.Text = model.Vulnerability;
            cmbStatus.SelectedItem = (Status)model.Status;
            txtComment.Text = model.Comment;

            Clipboard.SetText(model.SourceFile.Split(@"\").Last());
        }

        public void GetValuFromDGV(int id)
        {
            var temp = findings.FirstOrDefault(e => e.Id == id);

            if (temp != null)
                SetValues(temp);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (DGVForm == null || DGVForm.IsDisposed)
            {
                DGVForm = new frmDGV(this);
                DGVForm.Show();
            }
            else
            {
                DGVForm.Select();
            }
        }

        public List<TblAssessment> GetAssessments(bool filter = false)
        {
            if (filter)
                return filteredFindings;
            else
                return findings;
        }

        private void btnNxt_Click(object sender, EventArgs e)
        {
            if (useFiltered)
            {
                int index = filteredFindings.IndexOf(selectedItem);
                if (index < filteredFindings.Count - 1)
                    SetValues(filteredFindings[index + 1]);
                else
                {
                    if (MessageBox.Show("This is the last item if the filtered list" + Environment.NewLine + "Do you want to continue with full list?", "End of the list", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        index = findings.IndexOf(selectedItem);
                        if (index < findings.Count - 1)
                            SetValues(findings[index + 1]);
                    }
                }
            }
            else
            {
                int index = findings.IndexOf(selectedItem);
                if (index < findings.Count - 1)
                    SetValues(findings[index + 1]);
            }

            if (DGVForm != null && !DGVForm.IsDisposed)
                DGVForm.FindWithID(selectedItem.Id);
        }

        private void btnPrv_Click(object sender, EventArgs e)
        {
            if (useFiltered)
            {
                int index = filteredFindings.IndexOf(selectedItem);
                if (index > 0)
                    SetValues(filteredFindings[index - 1]);
                else
                {
                    if (MessageBox.Show("This is the first item if the filtered list" + Environment.NewLine + "Do you want to continue with full list?", "End of the list", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        index = findings.IndexOf(selectedItem);
                        if (index > 0)
                            SetValues(findings[index - 1]);
                    }
                }
            }
            else
            {
                int index = findings.IndexOf(selectedItem);
                if (index > 0)
                    SetValues(findings[index - 1]);
            }

            if (DGVForm != null && !DGVForm.IsDisposed)
                DGVForm.FindWithID(selectedItem.Id);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var temp = findings;

            if (!string.IsNullOrEmpty(txtClassification.Text))
                temp = temp.Where(e => e.Classification == txtClassification.Text).ToList();

            if (!string.IsNullOrEmpty(txtAPI.Text))
                temp = temp.Where(e => e.Api == txtAPI.Text).ToList();

            if (!string.IsNullOrEmpty(txtContext.Text))
                temp = temp.Where(e => e.Context == txtContext.Text).ToList();

            if (!string.IsNullOrEmpty(txtVulnerabiluty.Text))
                temp = temp.Where(e => e.Vulnerability == txtVulnerabiluty.Text).ToList();

            if (numLine.Value != 0)
                temp = temp.Where(e => e.LineNum == numLine.Value).ToList();

            if (!string.IsNullOrEmpty(txtSource.Text))
                temp = temp.Where(e => e.SourceFile.Contains(txtSource.Text)).ToList();

            if (cmbExcel.SelectedText == "True")
                temp = temp.Where(e => e.InExcel == true).ToList();
            if (cmbExcel.SelectedText == "False")
                temp = temp.Where(e => e.InExcel == false).ToList();

            if (!string.IsNullOrEmpty(txtComment2.Text))
                temp = temp.Where(e => e.Comment.Contains(txtComment2.Text)).ToList();

            filteredFindings = temp;

            if (filteredFindings.Count > 0)
            {
                if (DGVForm != null && !DGVForm.IsDisposed)
                {
                    useFiltered = true;
                    DGVForm.SetDGVList(true);
                    SetValues(filteredFindings.First());
                }
            }
            else
            {
                if (DGVForm != null && !DGVForm.IsDisposed)
                {
                    useFiltered = false;
                    DGVForm.SetDGVList(false);
                    SetValues(findings.First());
                }
            }
        }

        private void SetSortOrder(int value)
        {
            switch (value)
            {
                case 1:
                    sortOrder.Add("Classification");
                    break;
                case 2:
                    sortOrder.Add("Vulnerability");
                    break;
                case 3:
                    sortOrder.Add("API");
                    break;
                case 4:
                    sortOrder.Add("Context");
                    break;
                case 5:
                    sortOrder.Add("Line");
                    break;
                case 6:
                    sortOrder.Add("Source");
                    break;
                case 7:
                    sortOrder.Add("Excel");
                    break;
                case 8:
                    sortOrder.Add("Status");
                    break;
                case 9:
                    sortOrder.Add("Comment");
                    break;
                default:
                    break;
            }
        }

        private void RemoveSortOrder(int value)
        {
            switch (value)
            {
                case 1:
                    sortOrder.Remove("Classification");
                    break;
                case 2:
                    sortOrder.Remove("Vulnerability");
                    break;
                case 3:
                    sortOrder.Remove("API");
                    break;
                case 4:
                    sortOrder.Remove("Context");
                    break;
                case 5:
                    sortOrder.Remove("Line");
                    break;
                case 6:
                    sortOrder.Remove("Source");
                    break;
                case 7:
                    sortOrder.Remove("Excel");
                    break;
                case 8:
                    sortOrder.Remove("Status");
                    break;
                case 9:
                    sortOrder.Remove("Comment");
                    break;
                default:
                    break;
            }
        }

        private void chkClassification_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClassification.Checked)
                SetSortOrder(1);
            else
                RemoveSortOrder(1);
        }

        private void chkVulnerablity_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVulnerablity.Checked)
                SetSortOrder(2);
            else
                RemoveSortOrder(2);
        }

        private void chkAPI_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAPI.Checked)
                SetSortOrder(3);
            else
                RemoveSortOrder(3);
        }

        private void chkContext_CheckedChanged(object sender, EventArgs e)
        {
            if (chkContext.Checked)
                SetSortOrder(4);
            else
                RemoveSortOrder(4);
        }

        private void chkLine_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLine.Checked)
                SetSortOrder(5);
            else
                RemoveSortOrder(5);
        }

        private void chkSource_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSource.Checked)
                SetSortOrder(6);
            else
                RemoveSortOrder(6);
        }

        private void chkExcel_CheckedChanged(object sender, EventArgs e)
        {
            if (chkExcel.Checked)
                SetSortOrder(7);
            else
                RemoveSortOrder(7);
        }

        private void chkStatus_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStatus.Checked)
                SetSortOrder(8);
            else
                RemoveSortOrder(8);
        }

        private void chkComment_CheckedChanged(object sender, EventArgs e)
        {
            if (chkComment.Checked)
                SetSortOrder(9);
            else
                RemoveSortOrder(9);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var temp = new List<TblAssessment>();
            if (sortOrder.Count > 0)
            {
                if (useFiltered)
                    temp = filteredFindings;
                else
                    temp = findings;

                foreach (var item in sortOrder)
                {
                    switch (item)
                    {
                        case "Classification":
                            temp = temp.OrderBy(x => x.Classification).ToList();
                            break;
                        case "Vulnerability":
                            temp = temp.OrderBy(x => x.Vulnerability).ToList();
                            break;
                        case "API":
                            temp = temp.OrderBy(x => x.Api).ToList();
                            break;
                        case "Context":
                            temp = temp.OrderBy(x => x.Context).ToList();
                            break;
                        case "Line":
                            temp = temp.OrderBy(x => x.LineNum).ToList();
                            break;
                        case "Source":
                            temp = temp.OrderBy(x => x.SourceFile).ToList();
                            break;
                        case "Excel":
                            temp = temp.OrderBy(x => x.InExcel).ToList();
                            break;
                        case "Status":
                            temp = temp.OrderBy(x => x.Status).ToList();
                            break;
                        case "Comment":
                            temp = temp.OrderBy(x => x.Comment).ToList();
                            break;
                        default:
                            break;
                    }
                }

                filteredFindings = temp;

                if (filteredFindings.Count > 0)
                {
                    if (DGVForm != null && !DGVForm.IsDisposed)
                    {
                        useFiltered = true;
                        DGVForm.SetDGVList(true);
                        SetValues(filteredFindings.First());
                    }
                }
                else
                {
                    if (DGVForm != null && !DGVForm.IsDisposed)
                    {
                        useFiltered = false;
                        DGVForm.SetDGVList(false);
                        SetValues(findings.First());
                    }
                }
            }
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFromSetValue)
            {
                switch ((short)cmbStatus.SelectedItem)
                {
                    case 1:
                        if (!txtComment.Text.ToLower().Contains("pending"))
                        {
                            txtComment.Text = "Pending. " + txtComment.Text;
                        }
                        break;
                    case 2:
                        if (!(txtComment.Text.ToLower().Contains("false") && txtComment.Text.ToLower().Contains("positive")))
                        {
                            txtComment.Text = "False Positive. " + txtComment.Text;
                        }
                        break;
                    case 3:
                        if (!(txtComment.Text.ToLower().Contains("positive") && txtComment.Text.ToLower().Contains("pending")))
                        {
                            txtComment.Text = "Positive but pending. " + txtComment.Text;
                        }
                        break;
                    case 4:
                        if (!txtComment.Text.ToLower().Contains("remediated"))
                        {
                            txtComment.Text = "Remediated. " + txtComment.Text;
                        }
                        break;
                    default:
                        break;
                }

                selectedItem.Status = (short)cmbStatus.SelectedItem;
                selectedItem.Comment = txtComment.Text;

                using (var context = new IBMScanDBContext())
                {
                    context.Entry(selectedItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    var response = context.SaveChanges();

                    if (response > 0 && DGVForm != null && !DGVForm.IsDisposed)
                        if (useFiltered)
                            DGVForm.SetDGVList(true);
                        else
                            DGVForm.SetDGVList(false);
                } 
            }else
                isFromSetValue = false;
        }

        private void txtComment_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtComment_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && !string.IsNullOrEmpty(txtComment.Text))
            {
                ChangeComment();
            }
        }

        private void ChangeComment()
        {
            selectedItem.Comment = txtComment.Text;

            using (var context = new IBMScanDBContext())
            {
                context.Entry(selectedItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                var response = context.SaveChanges();

                if (response > 0)
                    if (useFiltered)
                        DGVForm.SetDGVList(true);
                    else
                        DGVForm.SetDGVList(false);
            }
        }

        private void txtComment_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtComment.Text))
                ChangeComment();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ChangeComment();
        }
    }
}
