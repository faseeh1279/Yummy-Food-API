using Microsoft.EntityFrameworkCore;
using Yummy_Food_API.Repositories.Interfaces; 

namespace Yummy_Food_API.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly ApplicationDBContext _context;
        public MenuItemRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<MenuItem>> GetMenuItemsAsync()
        {
            return await _context.MenuItems.Include(m => m.ItemCategory).ToListAsync();
            // Inlcude means also load related Item Category with each Menu Item.
        }
    }
}