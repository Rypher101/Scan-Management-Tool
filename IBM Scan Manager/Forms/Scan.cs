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

namespace IBM_Scan_Manager.Forms
{
    public partial class frmScan : Form
    {
        private readonly int _projID;
        private frmViewFindings findingsForm = null;

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        public frmScan(int ProjID)
        {
            _projID = ProjID;
            InitializeComponent();
        }

        private void Scan_Load(object sender, EventArgs e)
        {
            cmbScan.DropDownStyle = ComboBoxStyle.DropDownList;

            using (var context = new IBMScanDBContext())
            {
                var project = context.TblProjects.FirstOrDefault(e=>e.Id == _projID);

                if (project == null)
                    this.Close();

                lblProject.Text = "Project and Module : " + project.ProjName + " - " + project.ModuleName;
            }

            FillCmb();
        }

        private void FillCmb()
        {
            using(var context = new IBMScanDBContext())
            {
                var scan = context.TblScans.OrderByDescending(e=>e.ScanNum).Where(e=>e.ProjId == _projID).ToList();
                cmbScan.Items.Clear();

                if (scan.Count > 0)
                {
                    foreach (var item in scan)
                    {
                        var temp = new ComboboxItem()
                        {
                            Text = item.ScanType + " - " + item.ScanNum.ToString(),
                            Value = item.Id
                        };

                        cmbScan.Items.Add(temp);
                    }

                    cmbScan.SelectedIndex = 0;
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new IBMScanDBContext())
            {
                var scan = context.TblScans.Where(e=>e.ProjId == _projID && e.ScanType == cmbNewScan.Text).ToList();

                if (scan.Count > 0)
                    nmbScan.Value = scan.Max(e => e.ScanNum) + 1;
                else
                    nmbScan.Value = 1;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var context = new IBMScanDBContext())
            {
                var scan = new TblScan()
                {
                    ProjId = _projID,
                    ScanType = cmbNewScan.Text,
                    ScanDate = dtpScan.Value,
                    ScanNum = (int)nmbScan.Value
                };

                context.TblScans.Add(scan);
                var response = context.SaveChanges();

                if (response>0)
                {
                    FillCmb();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new frmNewAssessment(int.Parse((cmbScan.SelectedItem as ComboboxItem).Value.ToString())).Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new frmNewExcel(int.Parse((cmbScan.SelectedItem as ComboboxItem).Value.ToString())).Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (findingsForm == null || findingsForm.IsDisposed)
            {
                findingsForm = new frmViewFindings(int.Parse((cmbScan.SelectedItem as ComboboxItem).Value.ToString()));
                findingsForm.Show();
            }
            else
            {
                findingsForm.Select();
            }
            
        }
    }
}
