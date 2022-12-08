namespace MarkdownToSiteGenerator.Markdown
{
    internal class OrderedListItemParser : ParentMarkdownSymbolParser
   {

        protected override string RegexStr => @"(^[\s-[\r\n]]*[\d]+\.[\s-[\r\n]]+)([^(?:\n|\r]*)(\r\n|\r|\n|$)";

        public override SymbolisedTextWithChildren ToSymbolisedText(SymbolLocation location) => new ListItem(location, true);
    }
}
