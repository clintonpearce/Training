using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //** Pull in the file from the server **//
            //Unzip the file 

            string[] arrays;
            String sdira = @"\\csiadSAT07\page_content";
            arrays = Directory.GetFiles(sdira, "*", SearchOption.AllDirectories).Select(x => Path.GetFileName(x)).ToArray();

            //foreach (string s in arrays) {
                //System.Console.WriteLine(s);


                byte[] input = File.ReadAllBytes(@"\\csiadSAT07\page_content\20140720_17_ia1cid01_3_page_content_tran.bcp.gz");

                Console.WriteLine("BEFORE DECOMPRESS*****************");
                byte[] decomp = Decompress(input);

                
                string xn = System.Text.Encoding.UTF8.GetString(decomp);
                Debug.WriteLine(xn);


                //****DECODE 64****//
                //byte[] data = Convert.FromBase64String("H4sIAAAAAAAA/6tWykxRslIyMjAwVNJRKsgrCkotLs0pKVayio6t5QIABYyPeB4AAAA=");

                //** Decompress**//
               // byte[] decompress = Decompress(data);

                /** Convert to a string **/
                //string text = System.Text.ASCIIEncoding.ASCII.GetString(decompress);

                /** Write to console **/
               // System.Console.WriteLine(text);

           // }

            System.Console.ReadLine(); 
        }


        //** METHOD TO DECOMPRESS THE GZIP **//
        static byte[] Decompress(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip),
                          CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }


    }
}
