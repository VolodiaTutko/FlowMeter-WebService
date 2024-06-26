﻿namespace Application.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Meter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MeterId { get; set; }

        [Required]
        public string CounterAccount { get; set; }

        [Required]
        public string TypeOfAccount { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
