using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataModels;

[Table("rating_reviews")]
public partial class RatingReview
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
    [InverseProperty("RatingReviews")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("CustomerId")]
    [InverseProperty("RatingReviews")]
    public virtual Customer Customer { get; set; } = null!;
}
