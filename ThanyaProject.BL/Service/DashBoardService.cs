using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.BL.Service.IService;
using ThanyaProject.DAL.Repository.IRepository;
using ThanyaProject.Models.DTO;

namespace ThanyaProject.BL.Service
{
    public class DashBoardService: IDashBoardService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DashBoardService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<UserDashboardDto> GetUserDashboardAsync(int userId)
        {
            var devices = await _deviceRepository.GetDevicesByUserIdAsync(userId);

            return new UserDashboardDto
            {
                TotalDevices = devices.Count,
                OnlineDevices = devices.Count(d => d.Status == "Online"),
                OfflineDevices = devices.Count(d => d.Status == "Offline"),
                Devices = devices.Select(d => new DevicrDashboardDto
                {
                    DeviceId = d.DeviceId,
                    Name = d.Name,
                    Battery = d.Battery,
                    Status = d.Status,
                    Lat = d.Lat,
                    Long = d.Long,
                    LastUpdate = d.LastUpdate
                }).ToList()
            };
        }
        public async Task<AdminDashboardDto> GetAdminDashboardAsync()
        {
            var devices = await _deviceRepository.GetAllAsync();

            var deviceList = devices.ToList();

            return new AdminDashboardDto
            {
                TotalDevices = deviceList.Count,
                OnlineDevices = deviceList.Count(d => d.Status == "Online"),
                OfflineDevices = deviceList.Count(d => d.Status == "Offline"),
                Devices = deviceList.Select(d => new DevicrDashboardDto
                {
                    DeviceId = d.DeviceId,
                    Name = d.Name,
                    Battery = d.Battery,
                    Status = d.Status,
                    Lat = d.Lat,
                    Long = d.Long,
                    LastUpdate = d.LastUpdate
                }).ToList()
            };
        }
    }
}

