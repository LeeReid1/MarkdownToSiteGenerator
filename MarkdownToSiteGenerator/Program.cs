using MarkdownToSiteGenerator;

internal class Program
{
   private static async Task<int> Main(string[] args)
   {
      if (args.Length != 2)
      {
         Console.WriteLine("Please provide two arguments: a directory to read from, and a directory to write to");
         return -1;
      }

      string dir_from = CleanInput(args[0]);
      string dir_to = CleanInput(args[1]);

      SiteGenerator_FolderToFolder sg = new(dir_from, dir_to);

      if (sg.GetOutputFileLocations().ContainsDuplicates())
      {
         Console.WriteLine("Two or more files will be saved to the same location. Ensure all style, js, and image files have distinct file names, and that no source files have the same path but different extension");
         return -1;
      }

      FilePath[] toBeOverwritten = sg.GetWillBeOverwritten().ToArray();

      if (toBeOverwritten.Length != 0)
      {
         Console.WriteLine("--------");
         toBeOverwritten.ForEach(Console.WriteLine);
         Console.WriteLine("The above files will be deleted and regenerated. Continue? (y or n)");
         while (true)
         {
            var pressed = Console.ReadKey(true);
            if (pressed.KeyChar == 'y')
            {
               break;
            }
            else if (pressed.KeyChar == 'n')
            {
               return -1;
            }
         }

         sg.DeleteDestinationFiles();
      }

      await sg.Generate();

      Console.WriteLine("Done");
      return 0;
   }

   private static string CleanInput(string input)
   {
      if (!(input.EndsWith(Path.PathSeparator) || input.EndsWith(Path.AltDirectorySeparatorChar)))
      {
         input += Path.DirectorySeparatorChar;
      }
    
      return Path.GetFullPath(input);  
   }
}