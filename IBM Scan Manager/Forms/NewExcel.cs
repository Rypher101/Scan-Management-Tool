using IBM_Scan_Manager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static IBM_Scan_Manager.Classes.FindingStatus;

namespace IBM_Scan_Manager.Forms
{
    public partial class frmNewExcel : Form
    {
        private string file;
        private int scanID;

        public frmNewExcel(int scanID)
        {
            this.scanID = scanID;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ofd.Title = "Select HTML file";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                file = ofd.FileName;
                txtFIle.Text = ofd.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFIle.Text) || !txtFIle.Text.ToUpper().Contains(".XLSX"))
            {
                MessageBox.Show("Please select a valid file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ReadExcel();
        }

        private void ReadExcel()
        {
            var objList = new List<TblAssessment>();
            var dt = ConnectToExcel();
            bool first = true;
            int i = 0;

            AppendToLog("\tConverting to objects");
            foreach (DataRow item in dt.Rows)
            {
                if (first)
                {
                    first = false;
                    continue;
                }
                var temp = new TblAssessment()
                {
                    ScanId = scanID,
                    Classification = item.ItemArray[1].ToString(),
                    Vulnerability = item.ItemArray[4].ToString().Replace("Vulnerability.", ""),
                    Api = item.ItemArray[5].ToString(),
                    Context = item.ItemArray[6].ToString(),
                    LineNum = int.Parse(item.ItemArray[7].ToString()),
                    SourceFile = item.ItemArray[8].ToString(),
                    Comment = item.ItemArray[9].ToString()
                };

                if (temp.Comment.ToUpper().Contains("FALSE") && temp.Comment.ToUpper().Contains("POSITIVE"))
                    temp.Status = (short)Status.FalsePositive;
                else if (temp.Comment.ToUpper().Contains("REMEDIATED"))
                    temp.Status = (short)Status.Remediated;
                else if (temp.Comment == "")
                    temp.Status = (short)Status.NotReviewed;
                else
                    temp.Status = (short)Status.Doubtful;

                i++;
                objList.Add(temp);
            }
            AppendToLog("\tConverting to objects complete. Findings : " + objList.Count.ToString());

            var noDup = objList
                    .Where(e=>e.ScanId == scanID)
                    .GroupBy(e => new { e.Vulnerability, e.SourceFile, e.LineNum })
                    .Select(e => e.First()).ToList();

            var dupCount = objList.Count - noDup.Count;
            if (dupCount > 0)
                objList = noDup;
            i -= dupCount;
            AppendToLog("\tDuplicates found : " + dupCount.ToString());
            AppendToLog("\tTotal findings : " + i.ToString());
            noDup = null;

            if (objList.Count>0)
            {
                if (MessageBox.Show("Scan completed." + Environment.NewLine + "Total findings : " + i.ToString() + Environment.NewLine + "Do you want to store this data in the database?", "Scan Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    try
                    {
                        using (var context = new IBMScanDBContext())
                        {
                            var findingsDB = context.TblAssessments.Where(e => e.ScanId == scanID).ToList();
                            AppendToLog("\tFindign difference between excel and assessment");
                            foreach (var item in objList)
                            {
                                var temp = findingsDB.FirstOrDefault(e => e.Vulnerability == item.Vulnerability && e.SourceFile == item.SourceFile && e.LineNum == item.LineNum);
                                if (temp != null)
                                {
                                    temp.InExcel = true;
                                    if (temp.Status < item.Status && !string.IsNullOrWhiteSpace(item.Comment))
                                    {
                                        temp.Status = item.Status;
                                        temp.Comment = item.Comment;
                                    }

                                    if (string.IsNullOrWhiteSpace(temp.Api))
                                        temp.Api = item.Api;

                                    if (string.IsNullOrWhiteSpace(temp.Context))
                                        temp.Context = item.Context;

                                    context.Entry(temp).State = EntityState.Modified;
                                }
                            }
                            AppendToLog("\tSaving changes to database");
                            context.SaveChanges();
                            AppendToLog("\tSaving complete");
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendToLog("\tError : " + ex.Message.ToString(), true);
                    }
                }
            }

            AppendToLog("End process");
        }

        private DataTable ConnectToExcel()
        {
            string conn;
            DataTable dt = new DataTable();
            conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + file + "';Extended Properties='Excel 12.0;HDR=NO';";

            AppendToLog("Starting process");
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    AppendToLog("\tReading data from excel");
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [data$]", con);
                    oleAdpt.Fill(dt);
                    AppendToLog("\tReading data complete.");
                    return dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        private void AppendToLog(string txt, bool isError = false)
        {
            if (isError)
            {
                txtLog.SelectionColor = Color.Red;
            }
            txtLog.AppendText(txt);
            txtLog.SelectionColor = txtLog.ForeColor;
            txtLog.AppendText(Environment.NewLine);
            txtLog.ScrollToCaret();
        }

        private void NewExcel_Load(object sender, EventArgs e)
        {

        }
    }
}
