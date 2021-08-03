using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static IBM_Scan_Manager.Classes.FindingStatus;

namespace IBM_Scan_Manager.Forms
{
    public partial class frmDGV : Form
    {
        private frmViewFindings findingForm;
        private DataTable dt = new DataTable();
        public bool isFilteredList = false;

        public frmDGV(frmViewFindings findingForm)
        {
            this.findingForm = findingForm;
            InitializeComponent();
        }

        private void frmDGV_Load(object sender, EventArgs e)
        {
            dt.Columns.Add("#", typeof(Int32));
            dt.Columns.Add("Classification", typeof(string));
            dt.Columns.Add("Vulnerability", typeof(string));
            dt.Columns.Add("API", typeof(string));
            dt.Columns.Add("Context", typeof(string));
            dt.Columns.Add("Line", typeof(Int16));
            dt.Columns.Add("Source", typeof(string));
            dt.Columns.Add("Comment", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Excel", typeof(string));

            SetDGVList();
        }

        public void SetDGVList(bool filter = false)
        {
            var temp = findingForm.GetAssessments(filter);
            isFilteredList = filter;
            dt.Rows.Clear();

            foreach (var item in temp)
            {
                var tempRow = dt.NewRow();
                tempRow["#"] = item.Id;
                tempRow["Classification"] = item.Classification;
                tempRow["Vulnerability"] = item.Vulnerability;
                tempRow["API"] = item.Api;
                tempRow["Context"] = item.Context;
                tempRow["Line"] = item.LineNum;
                tempRow["Source"] = item.SourceFile;
                tempRow["Comment"] = item.Comment;
                tempRow["Status"] = (Status)item.Status;
                tempRow["Excel"] = item.InExcel ? "Yes" : "No";
                dt.Rows.Add(tempRow);
            }

            dgv.DataSource = dt;
            dgv.Refresh();
        }

        private void resizeColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public void FindWithID(int id)
        {
            var rows = dgv.Rows
                .OfType<DataGridViewRow>()
                .Where(e => (int)e.Cells[0].Value == id)
                .ToArray<DataGridViewRow>();

            if (rows.Length > 0)
                rows[0].Selected = true;
        }

        private void dgv_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Rows[e.RowIndex].Cells[0].Value != null)
            {
                findingForm.GetValuFromDGV((int)dgv.Rows[e.RowIndex].Cells[0].Value);
            }
        }
    }
}
