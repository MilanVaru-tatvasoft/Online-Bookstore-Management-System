using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataModels;

[Table("addtocart")]
public partial class Addtocart
{
    [Key]
    [Column("cartid")]
    public int Cartid { get; set; }

    [Column("customerid")]
    public int Customerid { get; set; }

    [Column("bookid")]
    public int Bookid { get; set; }

    [Column("isremoved")]
    public bool? Isremoved { get; set; }

    [Column("price")]
    [Precision(10, 2)]
    public decimal? Price { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [Column("totalamount")]
    [Precision(10, 2)]
    public decimal? Totalamount { get; set; }

    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime? CreatedDate { get; set; }

    [Column("updated_date", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedDate { get; set; }

    [ForeignKey("Bookid")]
    [InverseProperty("Addtocarts")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("Customerid")]
    [InverseProperty("Addtocarts")]
    public virtual Customer Customer { get; set; } = null!;
}
