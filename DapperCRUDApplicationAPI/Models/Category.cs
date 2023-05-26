using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfWorkRepositoryPattern.Core.Models
{
    public class Category 
    {
        public int Id { get; set; }
        [Required , MaxLength(250)]
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
