using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataModels;

[Table("orders")]
public partial class Order
{
    [Key]
    [Column("orderid")]
    public int Orderid { get; set; }

    [Column("customername")]
    [StringLength(100)]
    public string? Customername { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("phonenumber")]
    [StringLength(20)]
    public string? Phonenumber { get; set; }

    [Column("address")]
    [StringLength(500)]
    public string? Address { get; set; }

    [Column("city")]
    [StringLength(50)]
    public string? City { get; set; }

    [Column("customerid")]
    public int? Customerid { get; set; }

    [Column("orderdate", TypeName = "timestamp without time zone")]
    public DateTime Orderdate { get; set; }

    [Column("totalamount")]
    [Precision(10, 2)]
    public decimal Totalamount { get; set; }

    [Column("orderstatusid")]
    public int? Orderstatusid { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime? Createddate { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Column("modifiedby")]
    public int? Modifiedby { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [ForeignKey("Createdby")]
    [InverseProperty("OrderCreatedbyNavigations")]
    public virtual User? CreatedbyNavigation { get; set; }

    [ForeignKey("Customerid")]
    [InverseProperty("Orders")]
    public virtual Customer? Customer { get; set; }

    [ForeignKey("Modifiedby")]
    [InverseProperty("OrderModifiedbyNavigations")]
    public virtual User? ModifiedbyNavigation { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    [ForeignKey("Orderstatusid")]
    [InverseProperty("Orders")]
    public virtual Status? Orderstatus { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
