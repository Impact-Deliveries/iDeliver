﻿using IDeliverObjects.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.DTO
{
    public class BranchCaseDTO
    {

        public List<MerchantDeliveryPriceDTO>? MerchantDeliveryPrice { get; set; }
        public int? DeliveryStatus { get; set; }
        public decimal? DeliveryPriceOffer { get; set; }
    }
}
