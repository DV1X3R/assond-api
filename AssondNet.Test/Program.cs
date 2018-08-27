using System;
using System.Threading.Tasks;

namespace AssondNet.Test
{
    class Program
    {
        static void Main(string[] args)
            => MainAsync(args).GetAwaiter().GetResult();

        static async Task MainAsync(string[] args)
        {
            var assond = new AssondNet.Assond();

            Console.Write("Downloading words... ");
            await assond.DownloadWordsAsync();
            Console.WriteLine("Success!");
            Console.WriteLine("Random story line: " + assond.GetStoryLine());
            Console.WriteLine("Лира + Сасонд: " + assond.MixWordsAlchemy("лира", "сасонд"));
        }
    }
}
