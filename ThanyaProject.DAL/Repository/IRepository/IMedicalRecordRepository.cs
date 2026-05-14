using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.Models.Model;

namespace ThanyaProject.DAL.Repository.IRepository
{
    public interface IMedicalRecordRepository:  IRepository<MedicalRecord>
    {
        Task<MedicalRecord> GetMedicalRecordAsync(int UserId);
    }
}
