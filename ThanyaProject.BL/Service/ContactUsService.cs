using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.BL.Service.IService;
using ThanyaProject.DAL.Repository.IRepository;
using ThanyaProject.Models.DTO;
using ThanyaProject.Models.Model;

namespace ThanyaProject.BL.Service
{
    public class ContactUsService: IContactUsService
    {
        private readonly IRepository<ContactUs> _repo;

        public ContactUsService(IRepository<ContactUs> repo)
        {
            _repo = repo;
        }

        public async Task<ContactUs> SendAsync(ContactUsDto dto)
        {
            var contact = new ContactUs
            {
                Name = dto.Name,
                Email = dto.Email,
                Subject = dto.Subject,
                Message = dto.Message,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(contact);

            return contact;
        }

        public async Task<List<ContactUs>> GetAllAsync()
        {
            var result = await _repo.GetAllAsync();
            return result.ToList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var contact = await _repo.GetByIdAsync(id);

            if (contact == null)
                return false;

            await _repo.DeleteAsync(contact);
            return true;
        }
    }
}
