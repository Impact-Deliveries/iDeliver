﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.DTO.Notification
{
    public class Notification<T> where T : class
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; } = String.Empty;
        [JsonProperty("isAndroiodDevice")]
        public bool IsAndroiodDevice { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; } = String.Empty;
        [JsonProperty("body")]
        public string Body { get; set; } = String.Empty;
        [JsonProperty("module")]
        public short Module { get; set; } = (int)Enum.NotificationModule.DriverOrder;

        [JsonProperty("data")]
        public T? Data { get; set; }
    }
}
