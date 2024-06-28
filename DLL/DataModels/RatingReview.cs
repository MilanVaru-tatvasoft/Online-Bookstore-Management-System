using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataModels;

[Table("ratingreviews")]
public partial class Ratingreview
{
    [Key]
    [Column("rating_id")]
    public int RatingId { get; set; }

    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("book_id")]
    public int BookId { get; set; }

    [Column("rating")]
    public decimal? Rating { get; set; }

    [Column("reviews")]
    [StringLength(1000)]
    public string? Reviews { get; set; }

    [Column("rating_date", TypeName = "timestamp without time zone")]
    public DateTime? RatingDate { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("Ratingreviews")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("CustomerId")]
    [InverseProperty("Ratingreviews")]
    public virtual Customer Customer { get; set; } = null!;
}
