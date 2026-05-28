namespace WebAppMTGLogic.Models
{
    public class DeckLegalityResult
    { public DeckLegalityResult() { }
        public bool IsLegal { get; internal set; }
        public List<string> Errors { get; internal set; } = new List<string>();
    }
}