using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\data";

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
            int counter = 3;
            string line;
            while ( counter >0 && (line = file.ReadLine()) != null)
            {
                counter--;

                // Split line by tab delimiter
                String[] columns = line.Split('\t');

                //****DECODE 64****//
                byte[] data = Convert.FromBase64String(columns[15]);

                //** Decompress**//
                byte[] decompress = Decompress(data);

                /** Convert to a string **/
                string text = System.Text.ASCIIEncoding.ASCII.GetString(decompress);

                /** Write to console **/
                Console.WriteLine(text);
            }
            

            //Console.WriteLine("line : {0} ", line);
        }


        public static string Decompress(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
                return newFileName;
            }
        }
    }
}
