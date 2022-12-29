namespace MarkdownToSiteGenerator.Markdown
{
    public class MarkdownParseException : Exception
    {
        public MarkdownParseException(string message) : base(message) { }
        public MarkdownParseException(string message, MarkdownParseException inner) : base(message, inner) { }
    }
}
