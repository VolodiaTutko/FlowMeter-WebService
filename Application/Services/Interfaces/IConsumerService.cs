﻿namespace Application.Services.Interfaces
{
    using Application.DTOS;
    using Application.Models;

    public interface IConsumerService
    {
        public Task<Consumer> AddConsumer(Consumer consumer);

        Task<Consumer> DeleteConsumer(string id);

        public Task<List<Consumer>> GetList();

        Task<List<SelectConsumerDTO>> GetConsumerOptions();
    }
}
