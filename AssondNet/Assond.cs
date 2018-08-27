using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading.Tasks;

namespace AssondNet
{
    public class Assond
    {
        private string[] nouns, verbs, adjectives, adverbs;
        private Dictionary<string, string> alchemy;
        private Uri nounsUri = new Uri("https://docs.google.com/document/export?format=txt&id=1DELzpKy36MEXfLZiCWx_TC-8Mm4j7_xDbVRZVkVcBxE&token=AC4w5Vid4E1LcYwsurF9_Sz1zyw39I8K0g%3A1385401524463");
        private Uri verbsUri = new Uri("https://docs.google.com/document/export?format=txt&id=105UqJifXPK_TU6VOf118vB-pR5rsiCOCrQYGXUYh7Fg&token=AC4w5Vhre_DKEeuFrHXNgn6hxOUnCpOhJw%3A1385401819091");
        private Uri adjectivesUri = new Uri("https://docs.google.com/document/export?format=txt&id=15z_qrRMMVB62dUNBlRyF55tGBGl6MwqbzbVP9htxoZs&token=AC4w5VhFCQVLTlH8tTh5ciIf9RpFicL_sg%3A1385401609985");
        private Uri adverbsUri = new Uri("https://docs.google.com/document/export?format=txt&id=1SmgHJfsCSfOieT1geckQuEgkZgsdJMvoGVuSo6BsFs8&token=AC4w5VjBzb11qI3GHdY2GYgm3geT9NsQyg%3A1385401857725");
        private Uri alchemyUri = new Uri("https://docs.google.com/document/export?format=txt&id=16Q3W4o3FMELoypVLv-Hpz-PZCGzOlXOw9ZzrwZKAjCw&token=AC4w5VhKXb60roKQBJphMSIsxQ2NKyYkXg%3A1535366663282");

        private static Random random = new Random(Guid.NewGuid().GetHashCode());

        public async Task DownloadWordsAsync()
        {
            var strTasks = new Task<string[]>[4];
            strTasks[0] = downloadArrayAsync(nounsUri);
            strTasks[1] = downloadArrayAsync(verbsUri);
            strTasks[2] = downloadArrayAsync(adjectivesUri);
            strTasks[3] = downloadArrayAsync(adverbsUri);

            var dictTasks = new Task<Dictionary<string, string>>[1];
            dictTasks[0] = downloadDictAsync(alchemyUri);

            // Task.WaitAll(strTasks); Task.WaitAll(dictTasks); // Useless but cool

            nouns = await strTasks[0];
            verbs = await strTasks[1];
            adjectives = await strTasks[2];
            adverbs = await strTasks[3];
            alchemy = await dictTasks[0];
        }

        private string[] removeNewLines(string[] src)
        {
            for (int i = 0; i < src.Length; i++)
                src[i] = Regex.Replace(src[i], @"\t|\n|\r", "");
            return src;
        }

        private async Task<string[]> downloadArrayAsync(Uri uri)
        {
            var webClient = new WebClient();
            var src = (await webClient.DownloadStringTaskAsync(uri)).
                Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            webClient.Dispose();

            var result = removeNewLines(src);

            return result;
        }

        private async Task<Dictionary<string, string>> downloadDictAsync(Uri uri)
        {
            var webClient = new WebClient();
            var src = (await webClient.DownloadStringTaskAsync(uri)).
                Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            webClient.Dispose();

            src = removeNewLines(src);

            var result = new Dictionary<string, string>();
            foreach (string line in src)
            {
                var entity = line.Split('=');
                result.Add(entity[0].ToLower(), entity[1].ToLower());
            }

            return result;
        }

        public string GetWord(AssondWord aWord, bool firstLetterUppercase = false)
        {
            string word = null;

            switch (aWord)
            {
                case AssondWord.Noun: word = nouns[random.Next(nouns.Length)]; break;
                case AssondWord.Verb: word = verbs[random.Next(verbs.Length)]; break;
                case AssondWord.Adjective: word = adjectives[random.Next(adjectives.Length)]; break;
                case AssondWord.Adverb: word = adverbs[random.Next(adverbs.Length)]; break;

                default:
                    throw new NotImplementedException();
            }

            if (firstLetterUppercase)
            {
                var t = char.ToUpper(word[0]);
                word = t + word.Substring(1);
            }

            return word;
        }

        public string GetStoryLine()
        {
            string story = null;

            switch (random.Next(9))
            {
                case 0: story = GetWord(AssondWord.Noun, true) + " " + GetWord(AssondWord.Verb); break;
                case 1: story = GetWord(AssondWord.Noun, true) + " " + GetWord(AssondWord.Verb) + " " + GetWord(AssondWord.Adverb) + "."; break;
                case 2: story = GetWord(AssondWord.Noun, true) + " " + GetWord(AssondWord.Adverb) + " " + GetWord(AssondWord.Verb) + "."; break;

                case 3: story = GetWord(AssondWord.Adjective, true) + " " + GetWord(AssondWord.Noun) + " " + GetWord(AssondWord.Verb) + "."; break;
                case 4: story = GetWord(AssondWord.Adjective, true) + " " + GetWord(AssondWord.Noun) + " " + GetWord(AssondWord.Verb) + " " + GetWord(AssondWord.Adverb) + "."; break;
                case 5: story = GetWord(AssondWord.Adjective, true) + " " + GetWord(AssondWord.Noun) + " " + GetWord(AssondWord.Adverb) + " " + GetWord(AssondWord.Verb) + "."; break;

                case 6: story = GetWord(AssondWord.Adverb, true) + " " + GetWord(AssondWord.Noun) + " " + GetWord(AssondWord.Verb) + "."; break;
                case 7: story = GetWord(AssondWord.Adverb, true) + " " + GetWord(AssondWord.Verb) + " " + GetWord(AssondWord.Noun) + "."; break;
                case 8: story = GetWord(AssondWord.Adverb, true) + " " + GetWord(AssondWord.Verb) + " " + GetWord(AssondWord.Adjective) + " " + GetWord(AssondWord.Noun) + "."; break;
            }

            return story;
        }

        public string[] GetAlchemyWords()
        {
            var words = new List<string>();

            foreach (var i in alchemy.Keys)
            {
                var entity = i.Split('+');
                words.Add(entity[0]);
                words.Add(entity[1]);
            }

            var result = words.Distinct().ToArray();

            return result;
        }

        public string MixWordsAlchemy(string firstWord, string secondWord)
        {
            string result = null;

            alchemy.TryGetValue(firstWord.ToLower() + "+" + secondWord.ToLower(), out result);
            if (result == null)
                alchemy.TryGetValue(secondWord.ToLower() + "+" + firstWord.ToLower(), out result);

            return result;
        }
    }

    public enum AssondWord
    {
        Noun, Verb, Adjective, Adverb
    }
}
