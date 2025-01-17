﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace IDeliverObjects.Objects
{
    public partial class Order
    {
        public Order()
        {
            DriverOrders = new HashSet<DriverOrder>();
        }

        public long Id { get; set; }
        public long MerchantBranchId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DeliveryAmount { get; set; }
        public short Status { get; set; }
        public string Note { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public long? MerchantDeliveryPriceId { get; set; }
        public string ClientName { get; set; }
        public string ClientNumber { get; set; }

        public virtual MerchantBranch MerchantBranch { get; set; }
        public virtual MerchantDeliveryPrice MerchantDeliveryPrice { get; set; }
        public virtual ICollection<DriverOrder> DriverOrders { get; set; }
    }
}