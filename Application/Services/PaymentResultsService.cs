using Application.DataAccess;
using Application.Models;
using Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PaymentResultsService : IPaymentResultsService
    {
        private readonly ILogger<PaymentResultsService> _logger;
        private readonly IPaymentResultsRepository _resultsRepository;
        public PaymentResultsService(IPaymentResultsRepository resultsRepository, ILogger<PaymentResultsService> logger)
        {
            _resultsRepository = resultsRepository;
            _logger = logger;
        }


        public async Task<Accrual> GetPaymentsResultById(int id)
        {
            return await _resultsRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Accrual>> GetPaymentsResultByPersonalAccount(string personalAccount)
        {
            return await _resultsRepository.GetByPersonalAccountAsync(personalAccount);

        }


    
        public async Task<List<Accrual>> GetListResults()
        {
            try
            {
                var all = await _resultsRepository.All();
                var filteredList = all.Where(item => item != null).ToList();
                _logger.LogInformation("Retrieved {Count} results from the database.", filteredList.Count);
                return filteredList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving results from the database.");
                throw;
            }
        }

      
    }
}
