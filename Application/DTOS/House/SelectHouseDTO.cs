namespace Application.DTOS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Application.Models;

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
