﻿using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Item>> GetAllItemsAsync(); 
    }
}
