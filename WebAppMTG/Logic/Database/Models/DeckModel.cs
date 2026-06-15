using System;
using System.Collections.Generic;
using System.Text;
using WebAppMTGLogic.API.Models;

namespace WebAppMTGLogic.Database.Models
{
    public class DeckModel
    {
        public List<DeckCard> Mainboard { get; set; } = new();
        public List<DeckCard> Sideboard { get; set; } = new();
        public string ItemId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
