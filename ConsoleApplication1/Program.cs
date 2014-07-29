using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            // This is the path to the .gz files.
            string path = @"\\csiadsat07\page_content2\";

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

        // Insert logic for processing found files here. 
        public static void ProcessFile(string filepath)
        {
            FileInfo fileToDecompress = new FileInfo(filepath);
            string newfile = Decompress(fileToDecompress);
            Console.WriteLine("NewPath : {0}", newfile);

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(newfile);
        
            string line;
            while ((line = file.ReadLine()) != null)
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

                    /** Write to text file **/
                    using (StreamWriter sw = new StreamWriter(@"C:\data\" + sep(currentDomain) + "InOutput.txt", true))
                    {
                        sw.WriteLine(text);
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
