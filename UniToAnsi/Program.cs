using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UniToAnsi
{
    class Program
    {
        static  string targetEncoding = "us-ascii";
        static  string fallbackString = "?";
        static  string outputFileInfix = "-ascii";

        static int Main(string[] args)
        {
            //Console.WriteLine("called with:");
            //foreach (var arg in args)
            //{
            //    Console.WriteLine(arg);
            //}
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: UniToAns <inputfile> <outputFile>");
                return 1;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("File not found: " + args[0]);
                return 1;   
            }

            string inputFile = args[0];
            string outputFile;
            if (args.Length == 1)
                outputFile = GenerateOutputFileName(args[0]);
            else //more than 1 argument, only take 2nd argument
                outputFile = args[1];

            return Process(inputFile, outputFile);
        }

        /// <summary>
        /// appends "-ascii" to the file name
        /// </summary>
        /// <param name="p">path to file</param>
        /// <returns></returns>
        private static string GenerateOutputFileName(string p)
        {
            return Path.Combine(Path.GetDirectoryName(p),Path.GetFileNameWithoutExtension(p) + outputFileInfix + Path.GetExtension(p));
        }

        private static int Process(string inputFile, string outputFile)
        {
            //Console.WriteLine("inputFile:'{0}'\nOutputFile:'{1}'", inputFile, outputFile);
            try
            {
                //write to sw
                using (var sw = File.CreateText(outputFile))
                {
                    //read from sr
                    using (var sr = File.OpenText(inputFile))
                    {
                        var encoder = ASCIIEncoding.GetEncoding(targetEncoding, new EncoderReplacementFallback(fallbackString), new DecoderExceptionFallback());
                        while (!sr.EndOfStream)
                        {
                            //convert UTF-8 to us-ascii, replacing characters
                            var b = encoder.GetBytes(sr.ReadLine());
                            sw.WriteLine(encoder.GetString(b));
                        }
                        Console.WriteLine("file processed. Press any key to exit.");
                        Console.ReadKey();
                        //completed successfully
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while processing:\n " + ex.Message);
                return 1;
            }
        }
    }
}
