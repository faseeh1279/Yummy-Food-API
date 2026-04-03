using System.ComponentModel.DataAnnotations;
using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API.Models.DTOs
{
    public class ItemCategoryDTO
    {
        public string Category { get; set; } = string.Empty;
    }
}
