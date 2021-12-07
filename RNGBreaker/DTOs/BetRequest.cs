using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNGBreaker.DTOs
{
    public class BetRequest
    {
        public int PlayerId { get; set; }
        public int Money { get; set; }
        public int Number { get; set; }
    }
}
