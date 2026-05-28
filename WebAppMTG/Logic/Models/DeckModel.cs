using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.API.Models;

namespace WebAppMTGLogic.Models
{
    public class DeckModel
    {
        public string Format { get; set; } = string.Empty;
        public List<DeckCard> Mainboard { get; set; } = new();
        public List<DeckCard> Sideboard { get; set; } = new();
    }
}
