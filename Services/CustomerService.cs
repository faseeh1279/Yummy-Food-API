using Microsoft.Identity.Client;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Repositories;
using Yummy_Food_API.Repositories.Interfaces;
using Yummy_Food_API.Services.Interfaces;

namespace Yummy_Food_API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task GetAllItemsListAsync()
        {
            var items = await _customerRepository.GetAllItemsAsync();
            var itemCategories = await _customerRepository.GetAllItemCategoriesAsync();
            var itemImages = await _customerRepository.GetAllItemsAsync();
        }


    }

}
