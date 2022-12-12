namespace MarkdownToSiteGenerator
{
    /// <summary>
    /// Represents markup+content for an item in an list of bullet points
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
