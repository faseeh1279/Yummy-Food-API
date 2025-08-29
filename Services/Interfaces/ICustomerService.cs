using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<Item>> GetAllItemAsync(); 
    }
}
