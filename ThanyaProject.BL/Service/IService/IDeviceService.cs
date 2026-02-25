using ThanyaProject.Models.DTO;
using ThanyaProject.Models.Model;

public interface IDeviceService
{
    Task<List<Device>> GetDevicesByUserIdAsync(int userId);
    Task<Device> CreateAsync(DeviceCreateDto dto, int userId);
    Task<bool> UpdateAsync(int id, DeviceCreateDto device, int userId);
    Task<bool> DeleteAsync(int Deviceid, int userId);
}