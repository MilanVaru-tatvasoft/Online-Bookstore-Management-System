using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataModels;

[Table("emaillog")]
public partial class Emaillog
{
    [Key]
    [Column("emaillogid")]
    public int Emaillogid { get; set; }

    [Column("userid")]
    public int? Userid { get; set; }

    [Column("message", TypeName = "character varying")]
    public string? Message { get; set; }

    [Column("issent")]
    public bool? Issent { get; set; }

    [Column("senddate", TypeName = "timestamp without time zone")]
    public DateTime? Senddate { get; set; }
}
