using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            // This is the path to the .gz files.
            string path = @"\\csiadsat07\page_content2\";
            ProcessFile(path);

            /**
            if (File.Exists(path))
            {
                // This path is a file
                ProcessFile(path);
            }
            else if (Directory.Exists(path))
            {
                // This path is a directory
                ProcessDirectory(path);
            }
            else
            {
                Console.WriteLine("{0} is not a valid file or directory.", path);
            }
            **/


        }

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

        // Process all files in the directory passed in, recurse on any directories  
        // that are found, and process the files they contain. 
        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        public static IEnumerable<string> ReadFile(string inputFile)
        {
            using (Stream s = new GZipInputStream(File.OpenRead(inputFile)))
            {
                using (var stream = new StreamReader(s))
                {
                    string sLine;
                    while ((sLine = stream.ReadLine()) != null)
                    {
                        yield return sLine;
                    }
                }
            }
            yield break;
        }


        // Insert logic for processing found files here. 
        public static void ProcessFile(string filepath)
        {
            /** Changes in decompression to speed up code **/
            foreach (var f in Directory.GetFiles(filepath, "*ri*"))
            {
                Console.WriteLine("Reading file " + f);
                var rows = ReadFile(f);


            /**
            FileInfo fileToDecompress = new FileInfo(filepath);
            string newfile = Decompress(fileToDecompress);
            Console.WriteLine("NewPath : {0}", newfile);

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(newfile);
            
             **/

                foreach (var line in rows)
                {

                    // Split line by tab delimiter
                    String[] columns = line.Split('\t');

                    //get the correct domain
                    string currentDomain = columns[9].ToString().ToLower().Trim();
                    //list of the domains we're checking against
                    String[] domainsArray = { "amazon", "craigslist", "aol", "linkedin", "netflix" };

                    //if the array contains the current domain decode/decompress and if column 15 is not null or empty
                    if (domainsArray.Any(currentDomain.Contains) && (String.IsNullOrEmpty(columns[15]) == false))
                    {
                        //****DECODE 64****//
                        byte[] data = Convert.FromBase64String(columns[15]);

                        //** Decompress**//
                        byte[] decompress = Decompress(data);

                        /** Convert to a string **/
                        string text = System.Text.ASCIIEncoding.ASCII.GetString(decompress);

                        //Console.WriteLine("Parsing html for domain : {0}",currentDomain);
                        IDomain parser;
                        String output = "";

                        if (currentDomain.Contains("amazon"))
                        {
                            Console.WriteLine("Initializing parser for amazon");
                            parser = new AmazonParser(text);
                            parser.parseName();
                            Person p = parser.getPerson();
                            if (p.NameExists)
                            {
                                output += columns[2] + "\t" + columns[6] + "\t" + currentDomain + "\t" + p.Name;
                                Console.WriteLine("Output : {0}" + output);

                                using (StreamWriter sw = new StreamWriter(@"C:\data\" + sep(currentDomain) + "Output.txt", true))
                                {
                                    sw.WriteLine(output);
                                }
                            }
                        }
                        else if (currentDomain.Contains("craigslist"))
                        {
                            parser = new CraigslistParser(text);
                            parser.parseEmail();
                            Person p = parser.getPerson();
                            if (p.EmailExists)
                            {
                                output += columns[2] + "\t" + columns[6] + "\t" + currentDomain + "\t" + "\t" + p.Email;
                                Console.WriteLine("Output : {0}" + output);

                                using (StreamWriter sw = new StreamWriter(@"C:\data\" + sep(currentDomain) + "Output.txt", true))
                                {
                                    sw.WriteLine(output);
                                }
                            }


                        }
                        if (currentDomain.Contains("linkedin"))
                        {
                            //Console.WriteLine("Initializing parser for amazon");
                            parser = new LinkedinParser(text);
                            parser.parseName();
                            Person p = parser.getPerson();
                            if (p.NameExists || p.EmailExists)
                            {
                                output += columns[2] + "\t" + columns[6] + "\t" + currentDomain + "\t" + p.Name + "\t" + "" + "\t" + p.Email + "\t" + p.Gender + "\t" + "";
                                Console.WriteLine("Output : {0}" + output);

                                using (StreamWriter sw = new StreamWriter(@"C:\data\" + sep(currentDomain) + "Output.txt", true))
                                {
                                    sw.WriteLine(output);
                                }
                            }
                        }
                        
                        if (currentDomain.Contains("aol"))
                        {
                            //Console.WriteLine("Initializing parser for amazon");
                            parser = new AOLParser(text);
                            parser.parseName();
                            parser.parseEmail();
                            parser.parseGender();
                            parser.parseUserName();

                            Person p = parser.getPerson();
                            if (p.NameExists)
                            {
                                output += columns[2] + "\t" + columns[6] + "\t" + currentDomain + "\t" + p.Name + "\t" + p.UserName + "\t" + p.Email + "\t" + p.Gender;
                                Console.WriteLine("Output : {0}" + output);

                                using (StreamWriter sw = new StreamWriter(@"C:\data\" + sep(currentDomain) + "Output.txt", true))
                                {
                                    sw.WriteLine(output);
                                }
                            }
                        }


                        
                        /** Write to text file
                        using (StreamWriter sw = new StreamWriter(@"C:\data\" + sep(currentDomain) + "InOutput.txt", true))
                        {
                            sw.WriteLine(text);
                        }

                        **/

                        //******* Search the files for the regular expression ***********// 
                        /*string[] files = Directory.GetFiles(@"C:\data\"+sep(currentDomain)+"InOutput.txt");
                        foreach (string fileName in files) {

                            System.Console.Write("{0,24}", fileName);
                            if (System.Text.RegularExpressions.Regex.IsMatch(s, amazonNamePattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                            {
                                System.Console.WriteLine("  (match for '{0}' found)", sPattern);
                            }
                            else
                            {
                                System.Console.WriteLine();
                            }  
                        }*/
                    }
                }
            }
        }

        public static string sep(string s)
        {
            //to return the domain
            int l = s.IndexOf(".");
            if (l > 0)
            {
                return s.Substring(0, l);
            }
            return "";
        }


        public static string Decompress(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {

                // This is where you want the .txt file to be stored.
                string newFileName = @"C:\data\" + fileToDecompress.Name;
                if (File.Exists(newFileName))
                {
                    //Console.WriteLine("*****FILE EXISTS*******");
                    return newFileName;
                }

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        Console.WriteLine("Decompressing...");
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
                return newFileName;
            }
        }
    }
}
