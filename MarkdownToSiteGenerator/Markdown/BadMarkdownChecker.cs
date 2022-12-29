namespace MarkdownToSiteGenerator.Markdown
{
   /// <summary>
   /// Designed to find markdown errors that the user may not be aware of
   /// </summary>
   internal abstract class BadMarkdownChecker : MarkdownSymbolParser
   {
      protected abstract string Message { get; }
      public override SymbolisedText ToSymbolisedText(SymbolLocation sl) => throw new InvalidOperationException();

      public void ThrowIfBadMarkdownFound(string source)
      {
         SymbolLocation? found = GetMatches(source).FirstOrNull(a=>true);
         if (found != null)
         {
            throw new MarkdownParseException(Message);
         }
      }
   }
}
