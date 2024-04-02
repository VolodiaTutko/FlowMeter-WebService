using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IPaymentResultsService
    {
        public Task<Accrual> GetPaymentsResultById(int id);

        Task<IEnumerable<Accrual>>  GetPaymentsResultByPersonalAccount(string personalAccount);

        public Task<List<Accrual>> GetListResults();
    }
}
