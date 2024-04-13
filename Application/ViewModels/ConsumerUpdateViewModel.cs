using Application.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class ConsumerUpdateViewModel
    {
        public string PersonalAccount { get; set; }

        public string ConsumerOwner { get; set; }

        public int NumberOfPersons { get; set; }

        public string ConsumerEmail { get; set; }
    }
}
