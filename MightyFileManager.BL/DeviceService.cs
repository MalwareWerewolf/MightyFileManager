using MightyFileManager.Infrastracture;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace MightyFileManager.BL
{
    public class DeviceService : IService
    {
        public readonly byte[] salt = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; // Must be at least eight bytes.

        #region Private Fields

        private WebClient _client;

        #endregion

        #region CTOR

        public DeviceService()
        {
            _client = new WebClient();
        }

        #endregion

        #region Public Methods

        public void DownloadFile(string url, string fileName, string fileExtension)
        {
            try
            {
                _client.DownloadFile(url, fileName + "." + fileExtension);
                Console.WriteLine("File downloaded");
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to download the file");
            }
        }

        public void DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                Console.WriteLine("Original file {0} deleted successfully", path);
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to delete the file");
            }
        }

        public void EncryptFile(string inputFile, string outputFile)
        {
            try
            {
                string password = @"myKey123"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                using (FileStream fsCrypt = new FileStream(cryptFile, FileMode.CreateNew))
                {
                    using (RijndaelManaged RMCrypto = new RijndaelManaged())
                    {
                        using (CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write))
                        {
                            using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                            {
                                int data;
                                while ((data = fsIn.ReadByte()) != -1)
                                    cs.WriteByte((byte)data);

                                fsIn.Close();
                                cs.Close();
                                fsCrypt.Close();
                            }
                        }
                    }
                }

                Console.WriteLine("File {0} encrypted", inputFile);
            }
            catch (Exception)
            {
                Console.WriteLine("Encryption failed!");
            }

        }

        public void DecryptFile(string inputFile, string outputFile)
        {
            string password = @"myKey123"; // Your Key Here

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] key = UE.GetBytes(password);

            try
            {
                using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
                {
                    using (RijndaelManaged RMCrypto = new RijndaelManaged())
                    {
                        using (CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read))
                        {
                            using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                            {
                                int data;
                                while ((data = cs.ReadByte()) != -1)
                                    fsOut.WriteByte((byte)data);

                                fsOut.Close();
                                cs.Close();
                                fsCrypt.Close();

                                Console.WriteLine("File {0} decrypted to {1}", inputFile, outputFile);
                            }
                        }
                    }
                }
            }

            catch (Exception)
            {
                Console.WriteLine("Unable to decrypt");
            }
        }

        public void DeleteAllDirectoryFiles(string inputDir)
        {
            string[] fileEntries = Directory.GetFiles(inputDir);

            try
            {
                foreach (string fileName in fileEntries)
                {
                    File.Delete(fileName);
                }

                Console.WriteLine("Files deleted");
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to delete files, provide a correct path");
            }

        }

        public void EncryptFileAES(string sourceFilename, string destinationFilename, string password, int iterations)
        {
            AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            // NB: Rfc2898DeriveBytes initialization and subsequent calls to   GetBytes   must be eactly the same, including order, on both the encryption and decryption sides.
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, iterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);

            using (FileStream destination = new FileStream(destinationFilename, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using (CryptoStream cryptoStream = new CryptoStream(destination, transform, CryptoStreamMode.Write))
                {
                    try
                    {
                        using (FileStream source = new FileStream(sourceFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            source.CopyTo(cryptoStream);
                            Console.WriteLine("File {0} encrypted to {1}", sourceFilename, destinationFilename);
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Error encoutered {0}", exception);
                    }
                }
            }
        }

        public void DecryptFileAES(string sourceFilename, string destinationFilename, string password, int iterations)
        {
            AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            // NB: Rfc2898DeriveBytes initialization and subsequent calls to   GetBytes   must be eactly the same, including order, on both the encryption and decryption sides.
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, iterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);

            using (FileStream destination = new FileStream(destinationFilename, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using (CryptoStream cryptoStream = new CryptoStream(destination, transform, CryptoStreamMode.Write))
                {
                    try
                    {
                        using (FileStream source = new FileStream(sourceFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            source.CopyTo(cryptoStream);
                        }

                        Console.WriteLine("{0} decrypted", sourceFilename);
                    }
                    catch (CryptographicException exception)
                    {
                        if (exception.Message == "Padding is invalid and cannot be removed.")
                            throw new ApplicationException("Universal Microsoft Cryptographic Exception", exception);
                        else
                            throw;
                    }
                }
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        #endregion
    }
}
