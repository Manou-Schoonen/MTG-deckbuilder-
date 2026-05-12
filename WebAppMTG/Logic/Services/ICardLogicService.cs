using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.Models;
using WebAppMTGDAL.Services;

namespace WebAppMTGLogic.Services
{
    public interface ICardLogicService
    {
        Task<List<CardReturnModel>> SearchCardsAsync(string standardSearch);
        Task<List<CardReturnModel>> AdvancedSearchAsync(AdvancedSearchModel search);
    }
}
