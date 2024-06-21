using System;
using System.Collections.Generic;
using DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataContext;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Addtocart> Addtocarts { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Emaillog> Emaillogs { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<RatingReview> RatingReviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=BookStore;Username=postgres;Password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Addtocart>(entity =>
        {
            entity.HasKey(e => e.Cartid).HasName("addtocart_pkey");

            entity.Property(e => e.Checkout).HasDefaultValueSql("false");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Book).WithMany(p => p.Addtocarts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("addtocart_bookid_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Addtocarts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("addtocart_customerid_fkey");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Adminid).HasName("admins_pkey");

            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("false");
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Authorid).HasName("authors_pkey");

            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Bookid).HasName("books_pkey");

            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");

            entity.HasOne(d => d.Author).WithMany(p => p.Books).HasConstraintName("fk_author");

            entity.HasOne(d => d.Category).WithMany(p => p.Books).HasConstraintName("books_categoryid_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("categories_pkey");

            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("customers_pkey");

            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Emaillog>(entity =>
        {
            entity.HasKey(e => e.Emaillogid).HasName("emaillog_pkey");

            entity.Property(e => e.Senddate).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("orders_pkey");

            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders).HasConstraintName("orders_customerid_fkey");

            entity.HasOne(d => d.Orderstatus).WithMany(p => p.Orders).HasConstraintName("orders_orderstatusid_fkey");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.Orderdetailid).HasName("orderdetails_pkey");

            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Book).WithMany(p => p.Orderdetails).HasConstraintName("orderdetails_bookid_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderdetails).HasConstraintName("orderdetails_orderid_fkey");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("payment_pkey");

            entity.Property(e => e.PaymentDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Customer).WithMany(p => p.Payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_customer_id_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_order_id_fkey");
        });

        modelBuilder.Entity<RatingReview>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("rating_reviews_pkey");

            entity.Property(e => e.RatingDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Book).WithMany(p => p.RatingReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rating_reviews_book_id_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.RatingReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rating_reviews_customer_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("role_pkey");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("status_pkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("false");

            entity.HasOne(d => d.Role).WithMany(p => p.Users).HasConstraintName("fk_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
