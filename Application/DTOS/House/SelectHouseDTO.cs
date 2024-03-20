using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS
{
    using Application.Models;
    using System.ComponentModel.DataAnnotations;

    public class SelectHouseDTO
    {
        public int HouseId { get; set; }

        public string HouseAddress { get; set; }

        public SelectHouseDTO(House house)
        {
            HouseId = house.HouseId;
            HouseAddress = house.HouseAddress;
        }
    }
}
