using ThanyaProject.Models.Model;

public interface IDeviceService
{
    Task<List<Device>> GetDevicesByUserIdAsync(int userId);
    Task<Device> CreateAsync(Device device, int userId);
    Task<bool> UpdateAsync(int id, Device device, int userId);
}