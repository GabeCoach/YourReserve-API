﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YourReservation.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class YourReservationEntities1 : DbContext
    {
        public YourReservationEntities1()
            : base("name=YourReservationEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<RestaurantContact> RestaurantContacts { get; set; }
        public virtual DbSet<RestaurantLocation> RestaurantLocations { get; set; }
        public virtual DbSet<RestaurantOwner> RestaurantOwners { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<RestaurantAbout> RestaurantAbouts { get; set; }
        public virtual DbSet<RestaurantReview> RestaurantReviews { get; set; }
    }
}
