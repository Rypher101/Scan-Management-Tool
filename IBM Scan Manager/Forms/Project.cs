using IBM_Scan_Manager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace IBM_Scan_Manager.Forms
{
    public partial class frmProject : Form
    {
        public frmProject()
        {
            InitializeComponent();
        }

        private void frmProject_Load(object sender, EventArgs e)
        {
            AutoCompleteProject();
            FillComboBox();

            cmbProject.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void AutoCompleteProject()
        {
            using (var _context = new IBMScanDBContext())
            {
                var col = new AutoCompleteStringCollection();
                var response = _context.TblProjects.OrderByDescending(e=>e.Id).ToList();

                foreach (var item in response)
                {
                    var value = item.ProjName.Replace("  ", "") + (string.IsNullOrWhiteSpace(item.ModuleName) ? "" : " - " + item.ModuleName.Replace("  ", ""));
                    cmbProject2.Items.Add(value);
                    col.Add(value);
                }

                if (response != null)
                    cmbProject2.SelectedIndex = 0;

                cmbProject2.AutoCompleteSource = AutoCompleteSource.CustomSource;
                cmbProject2.AutoCompleteCustomSource = col;
                cmbProject2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
        }

        private void FillComboBox()
        {
            using (var context = new IBMScanDBContext())
            {
                cmbProject.Items.Clear();
                var projectList = context.TblProjects.Select(e => e.ProjName).Distinct().ToList();

                foreach (var item in projectList)
                {
                    cmbProject.Items.Add(item);
                }

                if (projectList != null)
                {
                    cmbProject.SelectedIndex = 0;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProject.Text))
                MessageBox.Show("Please enter a project name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                using (var context = new IBMScanDBContext())
                {
                    var project = new TblProject() { ProjName = txtProject.Text };

                    context.Add(project);
                    var response = context.SaveChanges();

                    if (response > 0)
                    {
                        AutoCompleteProject();
                        FillComboBox();
                    }
                }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtModule.Text))
                MessageBox.Show("Please enter a module name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                using (var context = new IBMScanDBContext())
                {
                    var project = new TblProject() { ProjName = cmbProject.Text, ModuleName = txtModule.Text };

                    context.Add(project);
                    var response = context.SaveChanges();

                    if (response > 0)
                    {
                        AutoCompleteProject();
                        FillComboBox();
                    }
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbProject2.Text))
                MessageBox.Show("Please select a project", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                using (var context = new IBMScanDBContext())
                {
                    var split = cmbProject2.Text.Split(" - ");
                    var project = new TblProject();

                    if (split.Length > 1)
                        project = context.TblProjects.FirstOrDefault(e => e.ProjName == split[0] && e.ModuleName == split[1]);
                    else
                        project = context.TblProjects.FirstOrDefault(e => e.ProjName == split[0]);


                    if (project == null)
                        MessageBox.Show("Couldnt find related project form database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        new Scan(project.Id).Show();
                }
        }
    }
}
