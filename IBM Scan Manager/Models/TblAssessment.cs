using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace IBM_Scan_Manager.Models
{
    [Table("tblAssessment")]
    public partial class TblAssessment
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("ScanID")]
        public int ScanId { get; set; }
        [Column(TypeName = "text")]
        public string Classification { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Vulnerability { get; set; }
        [Column("API", TypeName = "text")]
        public string Api { get; set; }
        [Column(TypeName = "text")]
        public string Context { get; set; }
        public int LineNum { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string SourceFile { get; set; }
        [Column(TypeName = "text")]
        public string Comment { get; set; }
        public short Status { get; set; }
        public bool InExcel { get; set; }
        public bool ReadOnly { get; set; }

        [ForeignKey(nameof(ScanId))]
        [InverseProperty(nameof(TblScan.TblAssessments))]
        public virtual TblScan Scan { get; set; }
    }
}
