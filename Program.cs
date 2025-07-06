using Microsoft.EntityFrameworkCore;
using Yummy_Food_API;
using Yummy_Food_API.Repositories;
using Yummy_Food_API.Repositories.Interfaces;
using Yummy_Food_API.Services;
using Yummy_Food_API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Repositories 
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();

// Add Services 
builder.Services.AddScoped<IMenuItemService, MenuItemService>(); 

// Add Mappings 

// Database Injecting & Add Database context 
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "My Large API Docs";
        c.DisplayRequestDuration();
        c.DefaultModelsExpandDepth(-1); // Optional: hide schemas
        c.EnableFilter(); // ✅ Enables search filter box
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
