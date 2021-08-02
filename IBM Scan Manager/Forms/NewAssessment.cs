using IBM_Scan_Manager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace IBM_Scan_Manager.Forms
{
    public partial class frmNewAssessment : Form
    {
        string[] files;
        int scanID;

        public frmNewAssessment(int scanID)
        {
            this.scanID = scanID;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ofd.Title = "Select HTML file";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                files = ofd.FileNames;
                txtFIle.Text = "";
                bool first = true;

                foreach (var item in ofd.FileNames)
                {
                    if (first)
                    {
                        txtFIle.Text = txtFIle.Text + item;
                        first = false;
                    }
                    else
                        txtFIle.Text = txtFIle.Text + Environment.NewLine + item;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFIle.Text) || !txtFIle.Text.ToUpper().Contains(".HTML"))
            {
                MessageBox.Show("Please select a valid file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ReadAssessment();
        }

        private void ReadAssessment()
        {
            txtLog.Text = "";
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            var objList = new List<TblAssessment>();
            int tot = 0;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                AppendToLog("Begining process");
                AppendToLog("    Reading HTML");

                if (files.Length == 0)
                {
                    AppendToLog("\tNo files selected.", true);
                    return;
                }

                foreach (var item in files)
                {
                    if (!File.Exists(item))
                    {
                        AppendToLog("Couldn't find the file.", true);
                        return;
                    }

                    AppendToLog("    Scanning file : " + item.Split("\\").Last());
                    htmlDoc.Load(item);
                    if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
                    {
                        AppendToLog("\tWarning : There are some prase errors", true);
                    }
                    AppendToLog("\tHTML read successfull");

                    AppendToLog("\tSelecting table");
                    var tables = htmlDoc.DocumentNode.SelectNodes("//table").ToList();
                    var rows = new List<HtmlAgilityPack.HtmlNode>();
                    if (tables.Count > 5)
                    {
                        AppendToLog("\tTable found");

                        AppendToLog("\tCreating Objects");
                        if (tables[0].SelectSingleNode(".//td").InnerText.Contains("Prev"))
                        {
                            rows = tables[2].SelectSingleNode("tbody").Elements("tr").ToList();
                        }
                        else if (tables[0].SelectSingleNode(".//td").InnerText.Contains("Next"))
                        {
                            rows = tables[5].SelectSingleNode("tbody").Elements("tr").ToList();
                        }
                        else
                        {
                            rows = tables[4].SelectSingleNode("tbody").Elements("tr").ToList();
                        }

                        rows.RemoveAt(0);
                        int i = 0;
                        bool preRow = false;
                        foreach (var row in rows)
                        {
                            if (preRow)
                            {
                                preRow = false;
                                continue;
                            }

                            var data = row.Elements("td").ToList();
                            var temp = new TblAssessment()
                            {
                                ScanId = scanID,
                                Classification = data[1].InnerText,
                                Vulnerability = data[2].InnerText,
                                SourceFile = data[4].InnerText,
                                LineNum = int.Parse(data[5].InnerText.Replace(",", ""))
                            };

                            preRow = true;
                            objList.Add(temp);
                            i++;
                        }
                        tot += i;

                        AppendToLog("\tObjects created");
                        AppendToLog("\tFindings : " + i.ToString());
                        AppendToLog("    Scanning Completed");
                    }
                    else
                    {
                        AppendToLog("\tCouldn't find talbe", true);
                        return;
                    }
                }

                var noDup = objList
                    .Where(e=>e.ScanId == scanID)
                    .GroupBy(e => new { e.Vulnerability, e.SourceFile, e.LineNum })
                    .Select(e=>e.First()).ToList();

                int dupCount = objList.Count - noDup.Count;
                if ( dupCount > 0)
                    objList = noDup;
                AppendToLog("    Duplicates found : " + dupCount.ToString());
                noDup = null;
                tot -= dupCount;

                if (objList.Count > 0)
                {
                    if (MessageBox.Show("Scan completed." + Environment.NewLine + "Total findings : " + tot.ToString() + Environment.NewLine + "Do you want to store this data in the database?", "Scan Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        using (var context = new IBMScanDBContext())
                        {
                            var crntObj = context.TblAssessments.Where(e => e.ScanId == scanID).ToList();

                            if (crntObj.Count>0)
                            {
                                var duplicates = (from nwList in objList
                                                  join oldList in crntObj
                                                  on new { nwList.Vulnerability, nwList.LineNum, nwList.SourceFile } equals new { oldList.Vulnerability, oldList.LineNum, oldList.SourceFile }
                                                  select nwList).ToList();

                                objList = objList.Except(duplicates).ToList(); 
                            }

                            context.TblAssessments.AddRange(objList);
                            var response = context.SaveChanges();

                            if (response > 0)
                            {
                                MessageBox.Show("Data saved", "Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppendToLog("Error : " + ex.Message.ToString(), true);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                AppendToLog("Total findings : " + tot.ToString());
                AppendToLog("Process end");
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

        private void frmNewAssessment_Load(object sender, EventArgs e)
        {

        }
    }
}
