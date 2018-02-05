using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models
{
    public class Portfolio
    {
        [Key]
        public int PortfolioId { get; set; }

        public int ProductId { get; set; }
        public Products Product { get; set; }

        public decimal CoinsQuantity { get; set; }
    }
}
