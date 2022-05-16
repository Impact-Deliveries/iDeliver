using IDeliverObjects.Common;
using IDeliverObjects.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.DTO
{
    public class DeliveryPriceDTO
    {
        public long? MerchantBranchID { get; set; }
        public decimal? Amount { get; set; }
        public short? DeliveryStatus { get; set; }
        public List<MerchantDeliveryPriceDTO>? DeliveryPrice { get; set; }

    }
    public  class MerchantDeliveryPriceDTO
    {
        public long? Id { get; set; }
        public long? MerchantBranchId { get; set; }
        public long? LocationId { get; set; }
        public double? FromDistance { get; set; }
        public double? ToDistance { get; set; }
        public decimal? Amount { get; set; }
        public bool? IsDeleted { get; set; }
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime? ModifiedDate { get; set; }
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime? CreationDate { get; set; }

    }
}

