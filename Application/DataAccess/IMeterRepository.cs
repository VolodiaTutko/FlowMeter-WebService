﻿using Application.Models;

namespace Application.DataAccess
{
    public interface IMeterRepository
    {
        Task<Meter> GetByCounterAccountAsync(string id);

        Task<List<Meter>> All();

        Task<Meter> Add(Meter meter);

        Task<Meter> Update(Meter meter);

        Task<Meter> Delete(int id);
    }
}
