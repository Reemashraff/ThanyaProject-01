using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.BL.Service.IService
{
    public interface IDashBoardService
    {
        Task<T> GetDashboardAsync();
    }
}
