using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.API.Models;
using WebAppMTGLogic.Models;

namespace WebAppMTGLogic.Interfaces
{
    public interface ILegalityRule
    {
        string FormatName { get; }
        DeckLegalityResult Validate(DeckModel deck);
    }
}
