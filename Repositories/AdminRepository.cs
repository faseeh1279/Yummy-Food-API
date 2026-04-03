using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using Yummy_Food_API.Models.DTOs;
using Yummy_Food_API.Repositories.Interfaces;

namespace Yummy_Food_API.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly ApplicationDBContext _dbContext;
    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AdminRepository(ApplicationDBContext dbContext, IWebHostEnvironment environment,
        IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _environment = environment;
        _httpContextAccessor = httpContextAccessor;
    }

}
