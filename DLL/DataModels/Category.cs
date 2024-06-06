﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataModels;

[Table("categories")]
public partial class Category
{
    [Key]
    [Column("categoryid")]
    public int Categoryid { get; set; }

    [Column("categoryname")]
    [StringLength(100)]
    public string Categoryname { get; set; } = null!;
}
