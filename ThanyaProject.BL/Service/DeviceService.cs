using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.DAL.Repository.IRepository;
using ThanyaProject.Models.DTO;
using ThanyaProject.Models.Model;

namespace ThanyaProject.BL.Service
{
    public class DeviceService
    {
        public readonly IDeviceRepository _repository;
        public async Task<List<Device>> GetDevicesByUserIdAsync(int userId)
        {
            return await _repository.GetDevicesByUserIdAsync(userId);
        }

        public async Task<Device> CreateAsync(DeviceCreateDto dto, int userId)
        {
            var count = await _repository.GetDeviceCountByUserIdAsync(userId);

            var deviceId = $"D{(count + 1).ToString("D3")}";

            var device = new Device
            {
                DeviceId = deviceId,
                UserId = userId,
                Name = dto.Name,
                Battery = dto.Battery,
                Status = "Offline",
                Lat = 0,
                Long = 0,
                LastUpdate = DateTime.UtcNow
            };

            await _repository.AddAsync(device);

            return device;
        }

        public async Task<bool> UpdateAsync(int id, Device device, int userId)
        {
            var existing = await _repository.GetByIdAsync(id);

            if (existing == null || existing.UserId != userId)
                return false;

            existing.Name = device.Name;
            existing.Status = device.Status;
            existing.Battery = device.Battery;
            existing.Lat = device.Lat;
            existing.Long = device.Long;
            existing.LastUpdate = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);

            return true;
        }
    }
}
