﻿using Microsoft.EntityFrameworkCore;
using TwitterApiApp.Models;

namespace TwitterApiApp.Data
{
    public class DataContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Tweet> Tweets { get; set; }

        public DbSet<TweetReaction> TweetReactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tweet>().HasKey(p => p.Id);
            modelBuilder.Entity<Tweet>().HasOne(p => p.ParentTweet)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(p => p.ParentTweetId)
                .IsRequired(false);
            modelBuilder.Entity<Tweet>().Property(p => p.ParentTweetId).IsRequired(false);

            modelBuilder.Entity<Tweet>().HasOne(p => p.User)
                .WithMany(p => p.Tweets)
                .HasForeignKey(p => p.UserId)
                .IsRequired(true);

            modelBuilder.Entity<Tweet>().HasMany(p => p.TweetReactions)
                .WithOne(p => p.Tweet)
                .HasForeignKey(p => p.TweetId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>().HasKey(p => p.Id);
            modelBuilder.Entity<User>().HasMany(p => p.TweetReactions)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TweetReaction>().HasKey(p => p.Id);
        }
    }
}