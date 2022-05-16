using IDeliverObjects.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.DTO
{
    public class MerchantBranchDTO
    {
        public long? Id { get; set; }
        public long? MerchantId { get; set; }
        public string? MerchantName { get; set; }
        public string? BranchName { get; set; }
        public string? Overview { get; set; }
        public string? BranchPicture { get; set; }
        public long? LocationId { get; set; }
        public string? LocationName { get; set; }
        public string? Address { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Mobile { get; set; }
        public string? Phone { get; set; }
        public bool? IsActive { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal? DeliveryPriceOffer { get; set; }
        public short? DeliveryStatus { get; set; }
        public List<MerchantDeliveryPrice>? DeliveryPrice { get; set; }
        public List<Attachment>? Attachments { get; set; }
    }

 
}
