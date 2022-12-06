using MarkdownToSiteGenerator;

//if (args.Length != 2)
//{
//   Console.WriteLine("Please provide two arguments: a directory to read from, and a directory to write to");
//}




string dir_from = "C:\\Users\\leere\\Downloads\\temp\\markdown-test\\source";// args[0];
string dir_to = "C:\\Users\\leere\\Downloads\\temp\\markdown-test\\destination";// args[1];

SiteGenerator_FolderToFolder sg = new(dir_from, dir_to);

FilePath[] toBeOverwritten = sg.GetWillBeOverwritten().ToArray();

if (toBeOverwritten.Length != 0)
{
   Console.WriteLine("--------");
   toBeOverwritten.ForEach(Console.WriteLine);
   Console.WriteLine("The above files will be deleted and regenerated. Continue? (y or n)");
   while(true)
   {
      var pressed = Console.ReadKey(true);
      if(pressed.KeyChar == 'y')
      {
         break;
      }
      else if(pressed.KeyChar == 'n')
      {
         return;
      }
   }

   sg.DeleteDestinationFiles();
}

await sg.Generate();

//MarkdownFileConverter<string,string> markdownConverter = new(pathMap);

//foreach(string dir_src in allFiles.Where(PathMapper.SuffixIsMarkdown))
//{
//   markdownConverter.ConvertAndWriteHTML(dir_src);
//}

