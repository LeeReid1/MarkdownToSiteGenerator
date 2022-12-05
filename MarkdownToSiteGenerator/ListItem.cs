namespace MarkdownToSiteGenerator
{
    /// <summary>
    /// Represents an item in an ordered list of bullet points
    /// </summary>
    internal class ListItem : SymbolisedTextWithChildren
    {
        public bool Ordered { get; }
        public ListItem(SymbolLocation location, bool ordered) : base(location)
        {
            Ordered = ordered;
        }
    }
}
