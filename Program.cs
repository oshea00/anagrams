using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace anagrams
{
    class Program
    {
        static void Main(string[] args)
        {

            var lines = File.ReadAllLines(@"c:\users\mike\repos\anagrams\wordlist.txt")
                .Where(l=>(string.IsNullOrEmpty(l)==false && 
                           l.Any(c=>Char.IsLetter(c)==false)==false))
                .Select(l=>l.ToLower().Trim())
                .Distinct();

            var anagrams = new Dictionary<string,List<string>>(30000);

            foreach (var line in lines) {
                var key = DistToKeyString(FreqCount(line));
                if (!anagrams.ContainsKey(key)) {
                    anagrams.Add(key,new List<string>());
                }
                anagrams[key].Add(line);
            }

            foreach (var key in anagrams.Keys)
            {
                if (anagrams[key].Count>1)
                    System.Console.WriteLine(string.Join(" ",anagrams[key]));
            }

            System.Console.WriteLine("Most Anagrams:");
            var most = anagrams.Values.Select(v=>v.Count).Max();
            foreach (var item in anagrams.Values.Where(v=>v.Count==most)
                                                .Select(s=>string.Join(" ",s)))
            {
                System.Console.WriteLine(item);
            }
            
            System.Console.WriteLine("Largest words:");
            var biggest = anagrams.Values.Where(v=>v.Count>1)
                            .Select(v=>v[0].Length).Max();

            foreach (var item in anagrams.Values.Where(
                v=>(v[0].Length==biggest && v.Count>1)).Select(v=>v))
            {
                System.Console.WriteLine(string.Join(" ",item));
            }
        }

        static Dictionary<char,int> FreqCount(string word) {
            var freq = new Dictionary<char,int>(); 
            foreach (char c in word)
            {
                if (!freq.ContainsKey(c))
                    freq.Add(c,0);
                freq[c]++;
            }
            return freq;
        }

        static string DistToKeyString(Dictionary<char,int> dist) {
            return string.Join(":",dist.Keys.OrderBy(k=>k)
                                            .Select(k=>$"{k},{dist[k]}"));
        }
    }
}
