//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class RestaurantContact
    {
        public int RestaurantContactID { get; set; }
        public int RestaurantID { get; set; }
        public int RestaurantOwnerID { get; set; }
        public Nullable<int> RestaurantLocationID { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
