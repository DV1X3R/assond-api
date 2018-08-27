# Assond API

API was created as inside-joke to replace our team storyteller man.  
The logic is simple: download words from google docs (to easily add new fun words), and concatenate them into the sentences.

## Functions
* public string GetWord(AssondWord aWord, bool firstLetterUppercase = false) // Get random word (Noun, Verb, Adjective, Adverb)
* public string GetStoryLine() // Get random story
* public string[] GetAlchemyWords() // Array of words could be used for alchemy game
* public string MixWordsAlchemy(string firstWord, string secondWord) // Alchemy game (try to mix words and get the new one)

## Usage Example
```
var assond = new AssondNet.Assond(); // Create new 'Assond' instance
await assond.DownloadWordsAsync(); // Download words database
Console.WriteLine("Random story line: " + assond.GetStoryLine()); // random story
Console.WriteLine("Лира + Сасонд: " + assond.MixWordsAlchemy("лира", "сасонд")); // alchemy game

```
