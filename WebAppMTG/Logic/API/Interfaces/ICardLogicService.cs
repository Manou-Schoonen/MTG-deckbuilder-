using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGDAL.Services;
using WebAppMTGLogic.API.Models;

namespace WebAppMTGLogic.API.Interfaces
{
    public interface ICardLogicService
    {
        Task<List<CardReturnModel>> SearchCardsAsync(string standardSearch);
        Task<List<CardReturnModel>> AdvancedSearchAsync(AdvancedSearchModel search);
        Task<CardReturnModel?> GetCardByIdAsync(string id);
    }
}
