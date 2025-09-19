
using Ecommerce.Data.Models;

namespace Ecommerce.Data.Interfaces
{
    public interface IEcommerceStoreRepository
    {
        //using 'Task' is link a promise which also help us to work asynchronously

        Task<EcommerceStore> AddGoodsAsync(EcommerceStore goodsObj);
        Task<EcommerceStore> GetGoodsByIdAsync(int id);
        Task<List<EcommerceStore>> GetAllGoodsAsync();
        Task<EcommerceStore> UpdateGoodsAsync(int id, EcommerceStore updated);
        Task<string> DeleteGoodsByIdAsync(int id);
    }
}