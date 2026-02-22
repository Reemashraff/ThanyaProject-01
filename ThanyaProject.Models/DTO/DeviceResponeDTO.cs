using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ThanyaProject.Models.DTO
{
    public class DeviceResponeDTO
    {
        [JsonPropertyName("device_id")]
        public string DeviceId { get; set; }

        public string Name { get; set; }
        public string Status { get; set; }
        public int Battery { get; set; }

        [JsonPropertyName("last_location")]
        public LocationDto LastLocation { get; set; }

        [JsonPropertyName("last_update")]
        public string LastUpdate { get; set; }

    }
}
