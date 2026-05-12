using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebAppMTGDAL.Services
{
    public interface IScryfallService
    {
        Task<List<ScryfallCardData>> SearchCardsAsync(string standardSearch);
    }
}
