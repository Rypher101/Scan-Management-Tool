using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace IBM_Scan_Manager.Models
{
    [Table("tblProject")]
    public partial class TblProject
    {
        public TblProject()
        {
            TblScans = new HashSet<TblScan>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string ProjName { get; set; }
        [StringLength(50)]
        public string ModuleName { get; set; }

        [InverseProperty(nameof(TblScan.Proj))]
        public virtual ICollection<TblScan> TblScans { get; set; }
    }
}
