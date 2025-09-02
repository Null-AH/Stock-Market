using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace api.DTO.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol cannot be more than 10 letters")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MaxLength(30, ErrorMessage = "Company Name cannot be more than 30 letters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1,10000000000000)]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0.001,100)]
        public decimal LastDiv { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "Industry Name cannot be more than 25 letters")]
        public string Industry { get; set; } = string.Empty;

        [Required]
        [Range(1,500000000000)]
        public long MarketCap { get; set; }
    }
}