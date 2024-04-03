using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataAccess
{
    public interface IPaymentResultsRepository
    {
        Task<Accrual> GetByIdAsync(int id);

        Task<List<Accrual>> All();

        Task<IEnumerable<Accrual>> GetByPersonalAccountAsync(string personalAccount);
    }
}
