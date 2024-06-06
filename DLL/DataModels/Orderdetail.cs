using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataModels;

[Table("orderdetails")]
public partial class Orderdetail
{
    [Key]
    [Column("orderdetailid")]
    public int Orderdetailid { get; set; }

    [Column("orderid")]
    public int? Orderid { get; set; }

    [Column("bookid")]
    public int? Bookid { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("price")]
    [Precision(10, 2)]
    public decimal Price { get; set; }

    [Column("totalamount")]
    [Precision(10, 2)]
    public decimal? Totalamount { get; set; }

    [Column("createdby")]
    [StringLength(50)]
    public string? Createdby { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime? Createddate { get; set; }

    [ForeignKey("Bookid")]
    [InverseProperty("Orderdetails")]
    public virtual Book? Book { get; set; }

    [ForeignKey("Orderid")]
    [InverseProperty("Orderdetails")]
    public virtual Order? Order { get; set; }
}
