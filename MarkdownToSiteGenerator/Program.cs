using MarkdownToSiteGenerator;

if (args.Length != 2)
{
   Console.WriteLine("Please provide two arguments: a directory to read from, and a directory to write to");
}


string dir_from = args[0];
string dir_to = args[1];

PathMapper pathMap = new(dir_from, dir_to);


string[] allFiles = Directory.GetFiles(pathMap.Dir_SourceTop, "*", SearchOption.AllDirectories);

string[] toBeOverwritten = allFiles.Select(pathMap.GetDestination).Where(a=>File.Exists(a)).ToArray();

if (toBeOverwritten.Length != 0)
{
   Console.WriteLine("--------");
   toBeOverwritten.ForEach(Console.WriteLine);
   Console.WriteLine("The above files will be overwritten. Continue? (y or n)");
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

   toBeOverwritten.ForEach(File.Delete);
}


MarkdownFileConverter markdownConverter = new(pathMap);

foreach(string dir_src in allFiles.Where(PathMapper.SuffixIsMarkdown))
{
   markdownConverter.ConvertAndWriteHTML(dir_src);
}

