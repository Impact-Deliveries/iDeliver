using IDeliverObjects.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.DTO
{
    public class LocationDTO
    {
        public long? Id { get; set; }
        public string? Address { get; set; }
        public int? CountryId { get; set; }
        public string? City { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreationDate { get; set; }
    }

 
}
