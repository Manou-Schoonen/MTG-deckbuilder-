using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.API.Models;
using WebAppMTGLogic.Database.Models;

namespace WebAppMTGLogic.FormatRules.Interface
{
    public interface ILegalityRule
    {
        string FormatName { get; }
        int MainBoardCardLimit { get; }
        int SideBoardCardLimit { get; }
        int UniqueCardLimit { get; }
        DeckLegalityResult Validate(DeckModel deck);
    }
}
