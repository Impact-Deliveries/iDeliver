using IDeliverObjects.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.DTO
{
    public class MerchantEmployeeDTO
    {
        public long? Id { get; set; }
        public long? EnrolmentId { get; set; }
        public long? MerchantBranchId { get; set; }
        public string? NationalNumber { get; set; }
        public string? ProfilePicture { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? MerchantBranchName { get; set; }
        public string? Mobile { get; set; }
        public string? Phone { get; set; }
        public bool? IsActive { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Attachment>? Attachments { get; set; }
    }


}
