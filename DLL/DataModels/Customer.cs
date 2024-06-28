using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataModels;

[Table("customers")]
[Index("Email", Name = "customers_email_key", IsUnique = true)]
public partial class Customer
{
    [Key]
    [Column("customerid")]
    public int Customerid { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("passwordhash")]
    [StringLength(100)]
    public string Passwordhash { get; set; } = null!;

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("phonenumber")]
    [StringLength(20)]
    public string? Phonenumber { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime? Createddate { get; set; }

    [Column("userid")]
    public int? Userid { get; set; }

    [Column("city")]
    [StringLength(20)]
    public string? City { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Addtocart> Addtocarts { get; set; } = new List<Addtocart>();

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Customer")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [InverseProperty("Customer")]
    public virtual ICollection<Ratingreview> Ratingreviews { get; set; } = new List<Ratingreview>();
}
