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
    public class OrderDTO
    {

        public long? Id { get; set; }
        public long? MerchantDeliveryPriceID { get; set; }
        public long? MerchantBranchId { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? DeliveryAmount { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }
        public string? ClientName { get; set; }
        public string? ClientNumber { get; set; }
        public string? MerchantName { get; set; }
        public string? MerchantPhone { get; set; }
        public short? Status { get; set; }
        public string? Note { get; set; }
        public string? LocationName { get; set; }
        public long? LocationID { get; set; }
        public long? DriverID { get; set; }
        public long? EnrolmentID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? OrderDate { get; set; }
        public bool? IsDeleted { get; set; }



    }


}
