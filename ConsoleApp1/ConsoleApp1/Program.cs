using System;
using System.IO;
using System.IO.Compression;

namespace HelloApp
{
    class Program
    {
        private static string Compressing = @"c:/folder";
            static void Main(string[] args)
        {
            DirectoryInfo directorySelected = new DirectoryInfo(Compressing);
            if (directorySelected.Exists) // Проверка существования пути
            {
                Console.WriteLine(" Directory exist, working..."); 
                foreach (FileInfo fileToCompress in directorySelected.EnumerateFiles()) // Архивирование файлов
                {
                    if (!fileToCompress.FullName.Contains("gz"))
                    {
                        Compress(fileToCompress);
                    }
                }
                foreach (FileInfo fileToCompress in directorySelected.EnumerateFiles()) // Разархивирование файлов
                {
                    if (fileToCompress.FullName.Contains("gz"))
                    {
                        Decompress(fileToCompress);
                    }
                }
            }
            else // Рассуждение, что будет делать программа, в случае, если директория для работы не будет найдена
            {
                Console.WriteLine(" Drectory " + Compressing + " didn't found, do you want to create it? (y/n) ");
                string q; // Проверка ответа пользователя
                q = Console.ReadLine();
                if (q == "y")
                {
                    Directory.CreateDirectory(Compressing);
                    Console.WriteLine(" Directory has creted, please put the files and restart the program");
                }
                if (q == "n")
                {
                    Console.WriteLine(" Nothing has created, exiting...");
                }
            }
        }
        public static void Compress(FileInfo fileToCompress)
        {
            try
            {
                using (FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionLevel.Fastest))
                        {
                            originalFileStream.CopyTo(compressionStream);
                            Console.WriteLine($"Compressed: {fileToCompress.Name}");
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error to compress");
            }
        }
        public static void Decompress(FileInfo fileToDecompress)
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
                        Console.WriteLine($"Decompressed: {fileToDecompress.Name}");
                    }
                }
            }
        }

    }
}