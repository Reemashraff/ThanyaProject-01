using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.DAL.Data;
using ThanyaProject.Models.DTO;
using ThanyaProject.Models.Model;

namespace ThanyaProject.BL.Service
{
    using Microsoft.EntityFrameworkCore;
    using ThanyaProject.BL.Service.IService;
    using ThanyaProject.DAL.Data;
    using ThanyaProject.Models.DTO;
    using ThanyaProject.Models.Model;

    public class EmergencyContactService : IEmergencyContactService
    {
        private readonly AppDbContext _context;

        public EmergencyContactService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EmergancyContact> AddContact(int userId, EmergencyContactDto dto)
        {
            var contact = new EmergancyContact
            {
                UserId = userId,
                Name = dto.Name,
                Phone = dto.Phone,
            };

            _context.EmergancyContacts.Add(contact);
            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<EmergancyContact?> UpdateContact(int userId, int id, EmergencyContactDto dto)
        {
            var contact = await _context.EmergancyContacts
                .FirstOrDefaultAsync(x => x.ContactId == id && x.UserId == userId);

            if (contact == null)
                return null;

            contact.Name = dto.Name;
            contact.Phone = dto.Phone;

            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<bool> DeleteContact(int userId, int id)
        {
            var contact = await _context.EmergancyContacts
                .FirstOrDefaultAsync(x => x.ContactId == id && x.UserId == userId);

            if (contact == null)
                return false;

            _context.EmergancyContacts.Remove(contact);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<EmergancyContact>> GetAllContacts(int userId)
        {
            return await _context.EmergancyContacts
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}
