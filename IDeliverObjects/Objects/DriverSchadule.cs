﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace IDeliverObjects.Objects
{
    public partial class DriverSchadule
    {
        public long Id { get; set; }
        public long DriverId { get; set; }
        public int DayId { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Driver Driver { get; set; }
    }
}