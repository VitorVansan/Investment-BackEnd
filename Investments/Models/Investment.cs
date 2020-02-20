using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Investments.Models
{
    public class Investment
    {
        public string name { get; set; }
        public decimal buyPrice { get; set; }
        public decimal? sellPrice { get; set; }
        public DateTime dateBuy { get; set; }
        public DateTime? dateSell { get; set; }
        public decimal? feePrice { get; set; }

    }
}
