using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.Database.Models;
using WebAppMTGLogic.FormatRules.Interface;

namespace WebAppMTGLogic.FormatRules.Formats
{
    public class FakeLegalityRule : ILegalityRule
    {
        public string FormatName => "Standard";
        public int MainBoardCardLimit => 60;
        public int SideBoardCardLimit => 15;
        public int UniqueCardLimit => 4;

        public DeckLegalityResult ResultToReturn { get; set; }

        public DeckLegalityResult Validate(DeckModel deck)
        {
            return ResultToReturn;
        }
    }
}
