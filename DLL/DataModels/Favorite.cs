using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataModels;

[Table("favorites")]
public partial class Favorite
{
    [Key]
    [Column("favoriteid")]
    public int Favoriteid { get; set; }

    [Column("customerid")]
    public int Customerid { get; set; }

    [Column("bookid")]
    public int Bookid { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime? Createddate { get; set; }

    [ForeignKey("Bookid")]
    [InverseProperty("Favorites")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("Customerid")]
    [InverseProperty("Favorites")]
    public virtual Customer Customer { get; set; } = null!;
}
