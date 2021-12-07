using System;

namespace RNGBreaker.DTOs
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public int Money { get; set; }
        public DateTime DeletionTime { get; set; }
    }
}
