using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace IBM_Scan_Manager.Models
{
    [Table("tblExcel")]
    public partial class TblExcel
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
        public bool? Remediated { get; set; }

        [ForeignKey(nameof(ScanId))]
        [InverseProperty(nameof(TblScan.TblExcels))]
        public virtual TblScan Scan { get; set; }
    }
}
