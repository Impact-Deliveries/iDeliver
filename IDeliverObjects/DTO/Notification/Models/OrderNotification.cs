using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDeliverObjects.Objects;

namespace IDeliverObjects.DTO.Notification.Models
{
    public class OrderNotification
    {
        public long OrderID { get; set; }
        public short Status { get; set; }
        public string? Note { get; set; }
        public string? ClientName { get; set; }
        public string? ClientNumber { get; set; }
        public long DriverID { get; set; }
        public short DriverOrderStatus { get; set; }
        public string? DriverOrderNote { get; set; }
        public long MerchantBranchID { get; set; }
        public string? MerchantName { get; set; }
    }
}
