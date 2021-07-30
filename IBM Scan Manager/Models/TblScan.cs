using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace IBM_Scan_Manager.Models
{
    [Table("tblScan")]
    public partial class TblScan
    {
        public TblScan()
        {
            TblAssessments = new HashSet<TblAssessment>();
            TblExcels = new HashSet<TblExcel>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("ProjID")]
        public int ProjId { get; set; }
        [Required]
        [StringLength(50)]
        public string ScanType { get; set; }
        public int ScanNum { get; set; }
        [Column(TypeName = "date")]
        public DateTime ScanDate { get; set; }
        [Column(TypeName = "text")]
        public string AssementFile { get; set; }
        [Column(TypeName = "text")]
        public string ExcelFile { get; set; }
        [Column("URL", TypeName = "text")]
        public string Url { get; set; }

        [ForeignKey(nameof(ProjId))]
        [InverseProperty(nameof(TblProject.TblScans))]
        public virtual TblProject Proj { get; set; }
        [InverseProperty(nameof(TblAssessment.Scan))]
        public virtual ICollection<TblAssessment> TblAssessments { get; set; }
        [InverseProperty(nameof(TblExcel.Scan))]
        public virtual ICollection<TblExcel> TblExcels { get; set; }
    }
}
