using IDeliverObjects.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.DTO
{
    public class MerchantDTO
    {
        public long? Id { get; set; }
        public long?OrganizationId { get; set; }
        public string? MerchantName { get; set; }
        public string? Overview { get; set; }
        public string? Mobile { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? Owner { get; set; }
        public string? OwnerNumber { get; set; }
        public string? Position { get; set; }
        public string? QutationNumber { get; set; }
        public List<Attachment>? Attachments { get; set; }
    }

 
}
