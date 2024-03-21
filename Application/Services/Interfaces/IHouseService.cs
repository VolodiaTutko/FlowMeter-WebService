﻿
namespace Application.Services.Interfaces
{
    using Application.DTOS;
    using Application.Models;

    public interface IHouseService
    {
        public Task<House> AddHouse(House house);

        public Task<List<House>> GetList();

        Task<List<SelectHouseDTO>> GetHouseOptions();
    }
}
