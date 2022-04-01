namespace KianaCord 
{
    internal class Program
    {
        static async Task Main()
        {
            Bot[] bots;

            Console.WriteLine("Paste path to the text file with bot tokens");
            string input = Console.ReadLine() + @"\tokens.txt";

            if (File.Exists(input))
            {
                Console.WriteLine("Creating bots");

                IEnumerable<string> lines = File.ReadLines(input);
                int count = lines.Count();
                bots = new Bot[count];

                int i = 0;
                foreach (string line in lines)
                {
                    Console.WriteLine($"{i}. {line}");
                    bots[i] = new Bot(line);
                    bots[i].MainAsync();

                    i++;
                }
            }
            else
            {
                Console.WriteLine("File not found");
                return;
            }

            Console.ReadKey();
        }
    }
}