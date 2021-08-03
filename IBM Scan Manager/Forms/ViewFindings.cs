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
        private TblAssessment selectedItem;
        private bool useFiltered = false;
        public frmViewFindings(int scanID =10)
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
            selectedItem = model;
            lblClassification.Text = model.Classification;
            lblContext.Text = model.Context;
            lblExcel.Text = model.InExcel ? "Yes" : "No";
            lblLine.Text = model.LineNum.ToString();
            lblAPI.Text = model.Api;
            lblSource.Text = model.SourceFile;
            lblVulnerability.Text = model.Vulnerability;
            cmbStatus.SelectedItem = model.Status;
            txtComment.Text = model.Comment;

            Clipboard.SetText(model.SourceFile.Split(@"\").Last());
        }

        public void GetValuFromDGV(int id)
        {
            var temp = findings.FirstOrDefault(e => e.Id == id);

            if(temp!=null)
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

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void btnNxt_Click(object sender, EventArgs e)
        {
            if (useFiltered)
            {
                int index = filteredFindings.IndexOf(selectedItem);
                if (index < filteredFindings.Count-1)
                    SetValues(filteredFindings[index + 1]);
                else
                {
                    if (MessageBox.Show("This is the last item if the filtered list" + Environment.NewLine + "Do you want to continue with full list?", "End of the list", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        index = findings.IndexOf(selectedItem);
                        if (index<findings.Count-1)
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

        private void label10_Click(object sender, EventArgs e)
        {

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
                if (index >0)
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
    }
}
