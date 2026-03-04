using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.Models.Model;

namespace ThanyaProject.DAL.Repository.IRepository
{
    public interface ICartItemRepository: IRepository<CartItem>
    {
        Task<List<CartItem>> GetUserCartAsync(int userId);
        Task<CartItem?> GetCartItemAsync(int userId, int productId);
        Task ClearCartAsync(int userId);
    }
}
