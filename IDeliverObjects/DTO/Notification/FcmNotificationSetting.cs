using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.DTO.Notification
{
    public class FcmNotificationSetting
    {
        public string SenderId { get; set; } = String.Empty;
        public string ServerKey { get; set; } = String.Empty;
    }
}
