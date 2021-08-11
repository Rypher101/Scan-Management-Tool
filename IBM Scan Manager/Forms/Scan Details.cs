using IBM_Scan_Manager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IBM_Scan_Manager.Classes.FindingStatus;

namespace IBM_Scan_Manager.Forms
{
    public partial class frmDetails : Form
    {
        private int scanID;
        public frmDetails(int scanID)
        {
            this.scanID = scanID;
            InitializeComponent();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void frmDetails_Load(object sender, EventArgs e)
        {
            using (var context = new IBMScanDBContext())
            {
                var tempScan = context.TblScans.Find(scanID);
                if (tempScan !=null)
                {
                    var tempProj = context.TblProjects.Find(tempScan.ProjId);
                    if (tempProj!=null)
                    {
                        this.Text = "Scan Detaila - " + tempProj.ProjName + " - " + (string.IsNullOrWhiteSpace(tempProj.ModuleName) ? "" : (tempProj.ModuleName + " - ")) + tempScan.ScanType + " - " + tempScan.ScanNum;
                    }
                }
            }

            lblTotal.Text = "";
            lblRemediated.Text = "";
            lblPNR.Text = "";
            lblNotRew.Text = "";
            lblFP.Text = "";
            lblDoubtful.Text = "";

            lblExcelFP.Text = "";
            lblExcelNR.Text = "";
            lblExcelPNR.Text = "";
            lblExcelRemediated.Text = "";
            lblExcelTotal.Text = "";

            GetDetails();
        }

        private void GetDetails()
        {
            var fullList = new List<TblAssessment>();
            using (var context = new IBMScanDBContext())
            {
                fullList = context.TblAssessments.Where(e => e.ScanId == scanID).ToList();
            }

            lblTotal.Text = fullList.Count().ToString();
            lblRemediated.Text = fullList.Where(e => e.Status == (short)Status.Remediated).Count().ToString();
            lblPNR.Text = fullList.Where(e => e.Status == (short)Status.PositiveNotRemediated).Count().ToString();
            lblNotRew.Text = fullList.Where(e => e.Status == (short)Status.NotReviewed).Count().ToString();
            lblFP.Text = fullList.Where(e => e.Status == (short)Status.FalsePositive).Count().ToString();
            lblDoubtful.Text = fullList.Where(e => e.Status == (short)Status.Doubtful).Count().ToString();

            fullList = fullList.Where(e => e.InExcel == true).ToList();

            lblExcelDoubtful.Text = fullList.Where(e => e.Status == (short)Status.Doubtful).Count().ToString();
            lblExcelFP.Text = fullList.Where(e => e.Status == (short)Status.FalsePositive).Count().ToString();
            lblExcelNR.Text = fullList.Where(e => e.Status == (short)Status.NotReviewed).Count().ToString();
            lblExcelPNR.Text = fullList.Where(e => e.Status == (short)Status.PositiveNotRemediated).Count().ToString();
            lblExcelRemediated.Text = fullList.Where(e => e.Status == (short)Status.Remediated).Count().ToString();
            lblExcelTotal.Text = fullList.Count().ToString();
                
        }
    }
}
