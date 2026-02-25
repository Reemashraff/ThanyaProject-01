using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.Models.DTO;

namespace ThanyaProject.BL.Service.IService
{
    public interface IDashBoardService
    {
        Task<UserDashboardDto> GetUserDashboardAsync(int userId);
        Task<AdminDashboardDto> GetAdminDashboardAsync();
    }
}
