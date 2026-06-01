using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGDAL.ScryfallAPI.Models;

namespace WebAppMTGDAL.ScryfallAPI.Services
{
    public interface IScryfallService
    {
        Task<List<ScryfallCardData>> SearchCardsAsync(string standardSearch);
        Task<ScryfallCardData?> GetCardByIdAsync(string id);
    }
}
