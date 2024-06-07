using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataModels;

[Table("books")]
public partial class Book
{
    [Key]
    [Column("bookid")]
    public int Bookid { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("authorid")]
    public int? Authorid { get; set; }

    [Column("bookphoto")]
    [StringLength(100)]
    public string? Bookphoto { get; set; }

    [Column("noofpages")]
    public int? Noofpages { get; set; }

    [Column("publisherid")]
    public int? Publisherid { get; set; }

    [Column("categoryid")]
    public int? Categoryid { get; set; }

    [Column("price")]
    [Precision(10, 2)]
    public decimal Price { get; set; }

    [Column("stockquantity")]
    public int Stockquantity { get; set; }

    [Column("isbn")]
    [StringLength(20)]
    public string? Isbn { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime? Createddate { get; set; }

    [Column("description")]
    [StringLength(1000)]
    public string? Description { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [InverseProperty("Book")]
    public virtual ICollection<Addtocart> Addtocarts { get; set; } = new List<Addtocart>();

    [ForeignKey("Authorid")]
    [InverseProperty("Books")]
    public virtual Author? Author { get; set; }

    [InverseProperty("Book")]
    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    [ForeignKey("Publisherid")]
    [InverseProperty("Books")]
    public virtual Publisher? Publisher { get; set; }
}
