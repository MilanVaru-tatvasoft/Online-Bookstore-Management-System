using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataModels;

[Table("authors")]
public partial class Author
{
    [Key]
    [Column("authorid")]
    public int Authorid { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("bio")]
    public string? Bio { get; set; }

    [Column("birthdate", TypeName = "timestamp without time zone")]
    public DateTime? Birthdate { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime? Createddate { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [InverseProperty("Author")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
