using System.Collections;
using System.Diagnostics;
using System.Text;
using HashTable;


string[] words;
using (StreamReader sr = new StreamReader("input3.txt"))
    words = sr.ReadToEnd().ToLower().Split(new char[] { ',',':',' ','.','?','!',';','<','=','>','-','1','2','3','4','5','6','7','8','9','0','/',
        '\"', '*', '(', ')','[', ']','\'','\n','\r','\\' }, StringSplitOptions.RemoveEmptyEntries);


Stopwatch sw = new Stopwatch();
sw.Start();
Dictionary(words);
sw.Stop();
Console.WriteLine($"Dictionary : {sw.ElapsedMilliseconds}");

sw.Reset();

sw.Start();
HeshTable(words);
sw.Stop();
Console.WriteLine($"HashTable : {sw.ElapsedMilliseconds}");


Console.ReadKey();


static void HeshTable(string[] words)
{
    OpenAddressHashTable<string, int> hashTable = new();
    foreach (var word in words)
    {
        if (hashTable.ContainsKey(word))
            hashTable[word]++;
        else
            hashTable.Add(word, 1);
    }
    foreach (var pair in (from pair in hashTable where pair.Value > 27 select pair))
    {
        hashTable.Remove(pair.Key);
    }
}

static void Dictionary(string[] words)
{
    Dictionary<string, int> dict = new();
    foreach (var word in words)
    {
        if (dict.ContainsKey(word))
            dict[word]++;
        else
            dict.Add(word, 1);
    }
    foreach (var pair in (from pair in dict where pair.Value > 27 select pair))
    {
        dict.Remove(pair.Key);
    }
}
