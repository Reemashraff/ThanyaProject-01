using ThanyaProject.Models.DTO;
using ThanyaProject.Models.Model;

namespace ThanyaProject.BL.Service.IService
{
    public interface IContactUsService
    {
        Task<ContactUs> SendAsync(ContactUsDto dto);
        Task<List<ContactUs>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}
