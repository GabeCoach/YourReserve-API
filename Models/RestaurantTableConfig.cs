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
    
    public partial class RestaurantTableConfig
    {
        public int TableConfigID { get; set; }
        public int RestaurantID { get; set; }
        public Nullable<int> TableNumber { get; set; }
        public Nullable<int> NumberOfSeats { get; set; }
        public Nullable<int> TableTypeID { get; set; }
    }
}
