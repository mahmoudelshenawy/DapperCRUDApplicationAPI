using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfWorkRepositoryPattern.Core.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
