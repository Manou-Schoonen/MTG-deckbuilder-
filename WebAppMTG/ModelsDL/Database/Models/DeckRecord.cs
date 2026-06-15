using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsDL.Database.Models
{
    public class DeckRecord
    {
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
