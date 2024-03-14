using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Models;

namespace Application.DataAccess
{
    public interface IHouseRepository
    {
        Task<House> GetByIdAsync(int id);
        Task<List<House>> All();
        Task<House> Add(House house);
        Task<House> Update(House house);
        Task<House> Delete(int id);

    }
}
