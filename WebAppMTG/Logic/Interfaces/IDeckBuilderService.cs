using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.Models;

namespace WebAppMTGLogic.Interfaces
{
    public interface IDeckBuilderService
    {
        Task<DeckModel> BuildDeckAsync(IEnumerable<CardDeckEntry> entries);
    }
}
