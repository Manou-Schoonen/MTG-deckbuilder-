namespace WebAppMTGLogic.Database.Models
{
    public class DeckLegalityResult
    { 
        public DeckLegalityResult() { }
        public bool IsLegal { get; set; } //{ get; internal set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}