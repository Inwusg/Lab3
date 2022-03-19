using System.Diagnostics;
using System.Text;
using HashTable;


string[] words;
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
var encoding = Encoding.GetEncoding(1251);
using (StreamReader sr = new StreamReader("input3.txt", encoding: encoding))
    words = sr.ReadToEnd().ToLower().Split(new char[] { ',',':',' ','.','?','!',';','<','=','>','-','1','2','3','4','5','6','7','8','9','0','/',
        '\"', '*', '(', ')','[', ']','\'','\n','\r','\\' }, StringSplitOptions.RemoveEmptyEntries);


Stopwatch sw = new Stopwatch();
sw.Start();
Dictionary(words);
sw.Stop();
Console.WriteLine($"Dictionary : {sw.ElapsedMilliseconds}");


Stopwatch sw2 = new Stopwatch();
sw2.Start();
HeshTable(words);
sw2.Stop();
Console.WriteLine($"HashTable : {sw2.ElapsedMilliseconds}");


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
