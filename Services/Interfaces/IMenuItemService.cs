namespace Yummy_Food_API.Services.Interfaces
{
    public interface IMenuItemService
    { 
        Task<List<MenuItem>> GetAllMenuItemsAsync(); 
    }
}