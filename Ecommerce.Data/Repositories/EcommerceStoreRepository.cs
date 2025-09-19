using Ecommerce.Data.Data;
using Ecommerce.Data.Interfaces;
using Ecommerce.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data.Repositories
{
    public class EcommerceStoreRepository : IEcommerceStoreRepository
    {
        // "private readonly" means this field is only set once (in the constructor) and cannot be modified later, ensuring safe and consistent use of DbContext.
        private readonly EcommerceStoreDbContext _context;

        // Constructor for the repository class.
        // Dependency Injection (DI) will automatically pass an instance of EcommerceStoreDbContext when this repository is created.
        public EcommerceStoreRepository(EcommerceStoreDbContext context)
        {
            // Store the injected DbContext instance in the private field "_context" so it can be used later in the repository methods (e.g., for querying or saving data to the database).
            _context = context;
        }



        /*---------------------------AddGoods-------------------------------- */
        public async Task<EcommerceStore> AddGoodsAsync(EcommerceStore goodsObj)
        {
            await _context.EcommerceStores.AddAsync(goodsObj);
            await _context.SaveChangesAsync();
            return goodsObj;
        }

        /*---------------------------DeleteGoodsUsingId-------------------------------- */
        public async Task<string> DeleteGoodsByIdAsync(int id)
        {
            var task = await _context.EcommerceStores.FindAsync(id);

            if (task == null)
            {
                return $"Goods with id:{id} does not exist";
            }
            _context.EcommerceStores.Remove(task);
            await _context.SaveChangesAsync();

            return $"Goods with Id:{id} deleted successfully";
        }

        /*---------------------------GetAllGoods-------------------------------- */
        public async Task<List<EcommerceStore>> GetAllGoodsAsync()
        {
            return await _context.EcommerceStores.ToListAsync();
        }

        /*---------------------------GetGoodsById-------------------------------- */
        public async Task<EcommerceStore?> GetGoodsByIdAsync(int id)
        {
            return await _context.EcommerceStores.FindAsync(id);
        }

        /*---------------------------UpdateGoods-------------------------------- */
        public async Task<EcommerceStore?> UpdateGoodsAsync(int id, EcommerceStore updated)
        {
            var task = await _context.EcommerceStores.FindAsync(id);
            if (task == null) return null;

            if (!string.IsNullOrWhiteSpace(updated.Product))
            {
                task.Product = updated.Product;
            }
            else
            {
                task.Product = task.Product;
            }

            if (!string.IsNullOrWhiteSpace(updated.Orders))
            {
                task.Orders = updated.Orders;
            }
            else
            {
                task.Orders = task.Orders;
            }

            if (!string.IsNullOrWhiteSpace(updated.Category))
            {
                task.Category = updated.Category;
            }
            else
            {
                task.Category = task.Category;
            }

            _context.EcommerceStores.Update(task);
            return task;
        }
    }
}