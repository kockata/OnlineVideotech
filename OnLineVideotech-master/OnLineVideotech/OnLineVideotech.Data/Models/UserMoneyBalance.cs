using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnLineVideotech.Data.Models
{
    public class UserMoneyBalance
    {
        public UserMoneyBalance()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}