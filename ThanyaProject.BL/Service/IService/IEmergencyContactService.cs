using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.Models.DTO;
using ThanyaProject.Models.Model;

namespace ThanyaProject.BL.Service.IService
{
   public interface IEmergencyContactService
    {
        Task<EmergancyContact> AddContact(int userId, EmergencyContactDto dto);
        Task<EmergancyContact?> UpdateContact(int userId, int id, EmergencyContactDto dto);
        Task<List<EmergancyContact>> GetAllContacts(int userId);
        Task<bool> DeleteContact(int userId, int id);
    }
}
