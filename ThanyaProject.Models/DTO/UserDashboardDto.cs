using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.Models.Model;

namespace ThanyaProject.Models.DTO
{
    public class UserDashboardDto
    {
            public int TotalDevices { get; set; }
            public int OnlineDevices { get; set; }
            public int OfflineDevices { get; set; }

            public List<DevicrDashboardDto> Devices { get; set; }
            public MedicalRecord? MedicalRecord { get; set; }
        
    }
}
