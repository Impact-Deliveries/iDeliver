using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IDeliverObjects.DTO.Notification
{
    public class GoogleNotification<T> where T : class
    {
        [JsonProperty("priority")]
        public string Priority { get; set; } = "high";
        [JsonProperty("data")]
        public DataPayload<T> Data { get; set; }
        [JsonProperty("notification")]
        public DataPayload<T> Notification { get; set; }
    }
}
