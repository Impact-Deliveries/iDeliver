﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace IDeliverObjects.Objects
{
    public partial class MerchantDeliveryPrice
    {
        public MerchantDeliveryPrice()
        {
            Orders = new HashSet<Order>();
        }

        public long Id { get; set; }
        public long MerchantBranchId { get; set; }
        public long? LocationId { get; set; }
        public double? FromDistance { get; set; }
        public double? ToDistance { get; set; }
        public decimal Amount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Location Location { get; set; }
        public virtual MerchantBranch MerchantBranch { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}