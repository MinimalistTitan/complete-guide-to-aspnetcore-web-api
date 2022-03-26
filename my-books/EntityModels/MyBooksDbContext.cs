﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace my_books.EntityModels
{
    public partial class MyBooksDbContext : DbContext
    {
        public MyBooksDbContext()
        {
        }

        public MyBooksDbContext(DbContextOptions<MyBooksDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Authors> Authors { get; set; }
        public virtual DbSet<BookAuthors> BookAuthors { get; set; }
        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<LogEvents> LogEvents { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<Publishers> Publishers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authors>(entity =>
            {
                entity.Property(e => e.FullName).IsRequired();
            });

            modelBuilder.Entity<BookAuthors>(entity =>
            {
                entity.ToTable("Book_Authors");

                entity.HasIndex(e => e.AuthorId, "IX_Book_Authors_AuthorId");

                entity.HasIndex(e => e.BookId, "IX_Book_Authors_BookId");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.BookAuthors)
                    .HasForeignKey(d => d.AuthorId);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookAuthors)
                    .HasForeignKey(d => d.BookId);
            });

            modelBuilder.Entity<Books>(entity =>
            {
                entity.HasIndex(e => e.PublisherId, "IX_Books_PublisherId");

                entity.Property(e => e.CoverUrl).IsRequired();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Genre).IsRequired();

                entity.Property(e => e.Title).IsRequired();

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublisherId);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<LogEvents>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<Logs>(entity =>
            {
                entity.Property(e => e.Exception).IsRequired();

                entity.Property(e => e.Level).IsRequired();

                entity.Property(e => e.LogEvent).IsRequired();

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.MessageTemplate).IsRequired();

                entity.Property(e => e.Properties).IsRequired();
            });

            modelBuilder.Entity<Publishers>(entity =>
            {
                entity.HasKey(e => e.PublisherId);

                entity.Property(e => e.Name).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}