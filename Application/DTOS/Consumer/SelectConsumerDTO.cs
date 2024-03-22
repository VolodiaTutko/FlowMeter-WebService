using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS
{
    public class SelectConsumerDTO
    {
        public string PersonalAccount { get; set; }


        public SelectConsumerDTO(Consumer consumer)
        {
            PersonalAccount = consumer.PersonalAccount;
            
        }
    }
}
