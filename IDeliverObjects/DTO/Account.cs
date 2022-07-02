using IDeliverObjects.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.DTO
{
    public partial class Account
    {
        public long EnrolmentID { get; set; }
        public long EmployeeID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsOnline { get; set; } = true;
        public short Status { get; set; }
        public OrganizationDTO? Organization { get; set; } = new OrganizationDTO();
        public EnrolmentDevice? Device { get; set; }
    }
}
