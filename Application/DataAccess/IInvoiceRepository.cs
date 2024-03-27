using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataAccess
{
    public interface IInvoiceRepository
    {
        Task<Receipt> GetByIdAsync(int id);

        Task<List<Receipt>> All();

    }
}
