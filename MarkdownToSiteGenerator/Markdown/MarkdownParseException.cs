namespace MarkdownToSiteGenerator.Markdown
{
    public class MarkdownParseException : Exception
    {
        public MarkdownParseException(string message) : base(message) { }
    }
}
