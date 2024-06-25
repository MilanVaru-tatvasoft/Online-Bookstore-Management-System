using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataModels;

[Table("status")]
public partial class Status
{
    [Key]
    [Column("status_id")]
    public int StatusId { get; set; }

    [Column("status_name")]
    [StringLength(100)]
    public string StatusName { get; set; } = null!;
}
