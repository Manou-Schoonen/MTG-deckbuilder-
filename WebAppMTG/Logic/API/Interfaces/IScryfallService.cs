using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.API.Models;

namespace WebAppMTGLogic.API.Interfaces
{
    public interface IScryfallService
    {
        Task<List<ScryfallCardData>> SearchCardsAsync(string standardSearch);
        Task<ScryfallCardData?> GetCardByIdAsync(string id);
    }
}
