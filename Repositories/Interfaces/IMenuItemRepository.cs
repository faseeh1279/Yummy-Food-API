namespace Yummy_Food_API.Repositories.Interfaces
{
    public interface IMenuItemRepository
    {
        Task<List<MenuItem>> GetMenuItemsAsync(); 
    }
}