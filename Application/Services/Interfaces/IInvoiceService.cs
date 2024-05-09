using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IInvoiceService
    {
        public Task<Receipt> GetInvoiceById(int id);

        Task<IEnumerable<Receipt>> GetInvoiceByPersonalAccount(string personalAccount);

        public Task<List<Receipt>> GetList();

        public Task<Receipt> Add(params object[] data);
    }
}
