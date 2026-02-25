using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.BL.Service.IService;

namespace ThanyaProject.BL.Service
{
    public class DashBoardService: IDashBoardService
    {
        public async Task<object> GetDashboardAsync()
        {
            var devices = await _repository.GetAllAsync();

            var total = devices.Count;
            var online = devices.Count(d => d.Status == "Online");
            var offline = devices.Count(d => d.Status == "Offline");
            var avgBattery = total > 0 ? devices.Average(d => d.Battery) : 0;

            return new
            {
                totalDevices = total,
                onlineDevices = online,
                offlineDevices = offline,
                averageBattery = avgBattery
            };
        }
    }
}
