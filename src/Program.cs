using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

[assembly: AssemblyTitle("relative-search")]
[assembly: AssemblyProduct("relative-search")]
[assembly: AssemblyCopyright("Copyright � Ryan Parnas 2016")]

namespace relative_search
{
  class Program
  {
    static void Main(string[] args)
    {
      byte b;
      if (args.Length < 3 || !File.Exists(args[0]) || args.Skip(1).Any(arg => !byte.TryParse(arg, out b)))
      {
        Console.WriteLine("Usage: relative-search <file> <byte1> <byte2>...");
        return;
      }

      Console.WriteLine("Searching...");
      Console.WriteLine();

      var bytes = File.ReadAllBytes(args[0]);
      var pattern = new List<byte?>();

      for (int i = 1; i < args.Length; i++)
      {
        pattern.Add(args[i] == "*" ? null : (byte?)byte.Parse(args[i]));
      }

      for (int i = 0; i < bytes.Length - pattern.Count + 1; i++)
      {
        var patternByteZero = pattern[0];
        var zero = bytes[i];
        var success = true;

        for (int j = 1; j < pattern.Count; j++)
        {
          if (!pattern[j].HasValue)
          {
            continue;
          }

          var patternByte = pattern[j].Value;
          var val = bytes[i + j];
          if ((zero - patternByteZero) != (val - patternByte))
          {
            success = false;
            break;
          }
        }

        if (success)
        {
          Console.WriteLine("0x" + i.ToString("x2"));
        }
      }

      Console.WriteLine();
      Console.WriteLine("Done.");
    }
  }
}
