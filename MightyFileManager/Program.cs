using System;
using System.IO;
using System.Linq;
using MightyFileManager.BL;
using MightyFileManager.Infrastracture;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MightyFileManager
{
    class Program
    {
        private static Random random = new Random();

        public const int iterations = 1042;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            var serviceProvider = new ServiceCollection()
                .AddTransient<IService, DeviceService>()
                .BuildServiceProvider();

            string password = "1234"; //this is not the best, change it later with a batter password

            int choice = -1;
            FileInfo fi;

            var MightyService = serviceProvider.GetService<IService>();

            var start = DateTime.UtcNow;

            try
            {
                do
                {
                    #region Menu

                    Console.WriteLine("Menu: ");
                    Console.WriteLine("0) Close the program");
                    Console.WriteLine("1) Download a file");
                    Console.WriteLine("2) Delete a file");
                    Console.WriteLine("3) Encrypt a file using Rijndael algorithm");
                    Console.WriteLine("4) Decrypt a file Rijndael algorithm");
                    Console.WriteLine("5) Delete all files in a directory");
                    Console.WriteLine("6) Encrypt all files in a folder in a folder and its sub folders and delete the old ones using Rijndael algorithm (USE IT WISELY, THIS IS DANGEROUS)");
                    Console.WriteLine("7) Decrypt all files in a folder in a folder and its sub folders and delete the old ones using Rijndael algorithm (USE IT WISELY, THIS IS DANGEROUS)");
                    Console.WriteLine("8) Encrypt a file in a folder using AES algorithm");
                    Console.WriteLine("9) Decrypt a file in a folder using AES algorithm");
                    Console.WriteLine("10) Encrypt all files in a folder and its sub folders and delete the old ones using AES algorithm (USE IT WISELY, THIS IS DANGEROUS)");
                    Console.WriteLine("11) Decrypt all files in a folder and its sub folders and delete the old ones using AES algorithm (USE IT WISELY, THIS IS DANGEROUS)");

                    #endregion

                    Console.Write("Choose an option: ");
                    fi.line = Console.ReadLine();

                    try
                    {
                        choice = Convert.ToInt32(fi.line);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("You did not enter a number");
                    }

                    switch (choice)
                    {

                        case 0:
                            start = DateTime.UtcNow;
                            break;

                        #region Download a file
                        case 1:

                            Console.Write("Enter an url to download a file: ");
                            fi.url = Console.ReadLine();

                            Console.Write("Enter a file name: ");
                            fi.fileName = Console.ReadLine();

                            Console.Write("Enter a valid file extension: ");
                            fi.fileExtension = Console.ReadLine();

                            start = DateTime.UtcNow;

                            MightyService.DownloadFile(fi.url, fi.fileName, fi.fileExtension);

                            break;
                        #endregion

                        #region Delete a file
                        case 2:

                            Console.Write("Enter a path and a file name of that path: ");
                            fi.path = Console.ReadLine();

                            start = DateTime.UtcNow;

                            MightyService.DeleteFile(fi.path);

                            break;
                        #endregion

                        #region Encrypt a file
                        case 3:

                            Console.Write("Enter a path: ");
                            fi.path = Console.ReadLine();

                            Console.Write("Enter a file name and an extension, for example test.mp3: ");
                            fi.fileName = Console.ReadLine();

                            start = DateTime.UtcNow;

                            MightyService.EncryptFile(fi.path, fi.fileName);

                            break;
                        #endregion

                        #region Decrypt a file
                        case 4:

                            Console.Write("Enter a path: ");
                            fi.path = Console.ReadLine();

                            Console.Write("Enter a file name and an extension, for example test.mp3: ");
                            fi.fileName = Console.ReadLine();

                            start = DateTime.UtcNow;

                            MightyService.DecryptFile(fi.path, fi.fileName);

                            break;
                        #endregion

                        #region Delete all files in a dir
                        case 5:

                            Console.Write("Enter a path: ");
                            fi.path = Console.ReadLine();

                            start = DateTime.UtcNow;

                            MightyService.DeleteAllDirectoryFiles(fi.path);

                            break;
                        #endregion

                        #region Encrypt all files in a dir
                        case 6:

                            Console.Write("Enter a path: ");
                            fi.path = Console.ReadLine();

                            start = DateTime.UtcNow;

                            string[] fileEntries = Directory.GetFiles(fi.path);

                            Parallel.ForEach(Directory.GetFiles(fi.path, "*", SearchOption.AllDirectories), fileName =>
                            {
                                MightyService.EncryptFile(fileName, fi.path + "\\" + RandomString(5) + Path.GetExtension(fileName));
                                MightyService.DeleteFile(fileName);
                            });

                            break;
                        #endregion

                        #region Decrypt all files in a dir
                        case 7:

                            Console.Write("Enter a path: ");
                            fi.path = Console.ReadLine();

                            start = DateTime.UtcNow;

                            string[] fileEntriesDecrypt = Directory.GetFiles(fi.path);

                            Parallel.ForEach(Directory.GetFiles(fi.path, "*", SearchOption.AllDirectories), fileName =>
                            {
                                MightyService.DecryptFile(fileName, fi.path + "\\" + RandomString(5) + Path.GetExtension(fileName));
                                MightyService.DeleteFile(fileName);
                            });

                            break;
                        #endregion

                        #region Encrypt a file using AES
                        case 8:

                            Console.Write("Enter a path with a file name of that path: ");
                            fi.path = Console.ReadLine();

                            Console.Write("Enter the second path: ");
                            fi.fileName = Console.ReadLine();

                            start = DateTime.UtcNow;

                            MightyService.EncryptFileAES(fi.path, fi.fileName, password, iterations);

                            break;
                        #endregion

                        #region Decrypt a file using AES
                        case 9:

                            Console.Write("Enter a path: ");
                            fi.path = Console.ReadLine();

                            Console.Write("Enter the second path: ");
                            fi.fileName = Console.ReadLine();

                            start = DateTime.UtcNow;

                            MightyService.DecryptFileAES(fi.path, fi.fileName, password, iterations);

                            break;
                        #endregion

                        #region Encrypt all files in a folder and subfolders using AES
                        case 10:

                            Console.Write("Enter a path: ");
                            fi.path = Console.ReadLine();

                            start = DateTime.UtcNow;

                            Parallel.ForEach(Directory.GetFiles(fi.path, "*", SearchOption.AllDirectories), f =>
                            {
                                MightyService.EncryptFileAES(f, fi.path + "\\" + RandomString(5) + Path.GetExtension(f), password, iterations);
                                MightyService.DeleteFile(f);
                            });

                            break;
                        #endregion

                        #region Decrypt all files in a folder and subfolders using AES
                        case 11:

                            Console.Write("Enter a path: ");
                            fi.path = Console.ReadLine();

                            start = DateTime.UtcNow;

                            Parallel.ForEach(Directory.GetFiles(fi.path, "*", SearchOption.AllDirectories), f =>
                            {
                                MightyService.DecryptFileAES(f, fi.path + "\\" + RandomString(5) + Path.GetExtension(f), password, iterations);
                                MightyService.DeleteFile(f);
                            });

                            break;
                        #endregion

                        default:
                            Console.WriteLine("You did not choose a valid case");
                            break;
                    }

                    var end = DateTime.UtcNow;
                    var ris = (end - start).TotalSeconds;

                    if(choice > 0 && choice < 13)
                        Console.WriteLine("Execution time {0} seconds", ris);

                    Console.WriteLine();
                }
                while (choice != 0);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error encoutered {0}", ex);
            }

            Console.WriteLine("Enter a key to exit");
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
        }

        #region Private Methods

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private struct FileInfo
        {
            public string url, fileName, fileExtension, path, line, num;

            public FileInfo(string link, string fn, string fe, string p, string l, string n)
            {
                url = link;
                fileName = fn;
                fileExtension = fe;
                path = p;
                line = l;
                num = n;
            }
        }
        #endregion
    }
}
