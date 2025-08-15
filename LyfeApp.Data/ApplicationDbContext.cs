using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace LyfeApp.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PostModel> Posts { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<LikeModel> Likes { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<FavoriteModel> Favorites { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and other model settings here if needed
            modelBuilder.Entity<UserModel>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<LikeModel>()
                .HasKey(l => new { l.PostId, l.UserId });

            modelBuilder.Entity<LikeModel>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);   //if a post is deleted, all likes are deleted

            modelBuilder.Entity<LikeModel>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            //here, deleting all likes of a user can make the app enter a cycle, and mayne a post that a user like
            //will have all its likes removed
            //so restrict allows there to be only one delete path to avoid going in cycles


            //difference between like and comment? a like is unique per user, but a user could have multiple
            //comments on the same post

            // modelBuilder.Entity<CommentModel>()
            //     .HasKey(l => new { c.PostId, c.UserId });

            modelBuilder.Entity<CommentModel>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);   //if a post is deleted, all comments are deleted

            modelBuilder.Entity<CommentModel>()
                .HasOne(l => l.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //favorites
            modelBuilder.Entity<FavoriteModel>()
                .HasKey(f => new { f.PostId, f.UserId });

            modelBuilder.Entity<FavoriteModel>()
                .HasOne(f => f.Post)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FavoriteModel>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

        }

    }

}