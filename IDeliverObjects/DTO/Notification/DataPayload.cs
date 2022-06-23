using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IDeliverObjects.DTO.Notification
{
    public class DataPayload
    {
        [JsonProperty("title")]
        public string Title { get; set; } = String.Empty;
        [JsonProperty("body")]
        public string Body { get; set; } = String.Empty;
    }
}
