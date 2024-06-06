using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataModels;

[Table("admins")]
[Index("Email", Name = "admins_email_key", IsUnique = true)]
[Index("Userid", Name = "admins_userid_key", IsUnique = true)]
public partial class Admin
{
    [Key]
    [Column("adminid")]
    public int Adminid { get; set; }

    [Column("firstname")]
    [StringLength(100)]
    public string Firstname { get; set; } = null!;

    [Column("lastname")]
    [StringLength(100)]
    public string Lastname { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("passwordhash")]
    [StringLength(100)]
    public string Passwordhash { get; set; } = null!;

    [Column("birthdate", TypeName = "timestamp without time zone")]
    public DateTime? Birthdate { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("phonenumber")]
    [StringLength(20)]
    public string? Phonenumber { get; set; }

    [Column("userid")]
    public int? Userid { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime? Createddate { get; set; }

    [Column("city")]
    [StringLength(50)]
    public string? City { get; set; }

    [Column("gender")]
    [StringLength(10)]
    public string? Gender { get; set; }
}
