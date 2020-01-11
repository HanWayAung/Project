using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LuckDrawSystem.Models
{
    public class PrizeWinner
    {
        public int Id { get; set; }

        [Required]
        public string PrizeType { get; set; }
        [Required]
        public string WinningNumber { get; set; }
        [Required(ErrorMessage = "First Login bro!")]
        public string UserId { get; set; }

    }
}