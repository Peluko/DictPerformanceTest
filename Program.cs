
using System.Diagnostics;


const int minKeyLength = 10;
const int maxKeyLength = 40;
const int numberOfKeys = 10;
const int numberOfLookups = 10_000_000;

const int minValueLength = 10;
const int maxValueLength = 80;


var keys = RandomGenerator.StringSequence(minKeyLength, maxKeyLength).Take(numberOfKeys).ToList();

var asList = keys
    .Select(k => (Key: k, Value: RandomGenerator.GenerateString(minValueLength, maxValueLength)))
    .ToArray();

var asDict = asList.ToDictionary(i => i.Key, i => i.Value);

var lookups = RandomGenerator.SequenceFromItems(keys).Take(numberOfLookups);

// As List/Array
var store1 = new List<string>();
var watch1 = Stopwatch.StartNew();
foreach(var l in lookups)
{
    var v = asList.First(i => i.Key == l).Value;
    store1.Add(v);
}
watch1.Stop();
Console.WriteLine($"List ticks: {watch1.ElapsedTicks} ({watch1.ElapsedMilliseconds} ms)");


// As Dict
var store2 = new List<string>();
var watch2 = Stopwatch.StartNew();
foreach(var l in lookups)
{
    var v = asDict[l];
    store1.Add(v);
}
watch2.Stop();
Console.WriteLine($"Dict ticks: {watch2.ElapsedTicks} ({watch2.ElapsedMilliseconds} ms)");


public static class RandomGenerator
{
    static private readonly Random s_rnd = new();

    static public string GenerateString(int minLength, int maxLength)
    {
        const string allowedChars = "abcdefghijklmnopqrstuvwzyzABCDEFGHIJKLMNOPQRSTUVWZYZñÑ1234567890-=[]{}_+'|;:/.,<>?`~ ";

        var length = s_rnd.Next(minLength, maxLength + 1);
        var str = "";
        for (var i = 0; i < length; i++)
        {
            str += allowedChars[s_rnd.Next(allowedChars.Length)];
        }
        return str;
    }

    static public IEnumerable<string> StringSequence(int minLength, int maxLength)
    {
        while (true)
        {
            yield return GenerateString(minLength, maxLength);
        }
    }

    static public IEnumerable<T> SequenceFromItems<T>(ICollection<T> items)
    {
        while (true)
        {
            yield return items.ElementAt(s_rnd.Next(items.Count));
        }
    }
}

