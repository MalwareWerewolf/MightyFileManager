using System;

namespace MightyFileManager.Infrastracture
{
    public interface IService : IDisposable
    {
        void DownloadFile(string url, string fileName, string fileExtension);

        void DeleteFile(string path);

        void EncryptFile(string inputFile, string outputFile);

        void DecryptFile(string inputFile, string outputFile);

        void DeleteAllDirectoryFiles(string inputDir);

        void EncryptFileAES(string sourceFilename, string destinationFilename, string password, int iterations);

        void DecryptFileAES(string sourceFilename, string destinationFilename, string password, int iterations);
    }
}
