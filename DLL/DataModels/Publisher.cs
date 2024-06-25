using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataModels;

[Table("publishers")]
public partial class Publisher
{
    [Key]
    [Column("publisherid")]
    public int Publisherid { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime? Createddate { get; set; }

    [InverseProperty("PublisherNavigation")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
