﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RZHD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RZHD.Data
{
    public class DatabaseContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Station> Stations { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<StationRestaurant> StationRestaurants { get; set; }

        public DatabaseContext(DbContextOptions options) : base (options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureStation(builder);
            ConfigureRestaurant(builder);
            ConfigureStationRestaurant(builder);
        }

        private void ConfigureStationRestaurant(ModelBuilder builder)
        {
            builder.Entity<StationRestaurant>()
                .HasKey(sr => new { sr.StationId, sr.RestaurantId });

            builder.Entity<StationRestaurant>()
                .HasOne(sr => sr.Station)
                .WithMany(sr => sr.DeliverRestaurants)
                .HasForeignKey(sr => sr.StationId);

            builder.Entity<StationRestaurant>()
                .HasOne(sr => sr.Restaurant)
                .WithMany(sr => sr.DeliverStations)
                .HasForeignKey(sr => sr.RestaurantId);
        }

        private void ConfigureRestaurant(ModelBuilder builder)
        {
            builder.Entity<Restaurant>()
                .HasKey(res => res.Id);

            builder.Entity<Restaurant>()
                .HasMany(res => res.DeliverStations);
        }

        private void ConfigureStation(ModelBuilder builder)
        {
            builder.Entity<Station>()
                .HasKey(st => st.Id);

            builder.Entity<Station>()
                .HasMany(st => st.DeliverRestaurants);
        }
    }
}
