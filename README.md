# MightyFileManager

**MightyFileManager** is a console application built to easily __delete, encrypt, decrypt__ files and many other features. There are also some **Ramsomware** funcionalities in 10 and 11 cases, my suggestion is to use these two commands wisely, if you should put the wrong path all files in that path and all its subfolders will be encrypt. The process is very quickly from the moment I used the parallelism, you can obviously stop the program execution by pressing **CTRL + C** but it could be too late, **so be careful**.

## Build instructions

Install [.Net Core 2.1 or later](https://dotnet.microsoft.com/download).

### Start the program on Windows

If you are on Windows, you just need to open the MightyFileManager.sln file with [Visual Studio](https://visualstudio.microsoft.com/) (obviously you need to download it first) and press CTRL + 5 to start the program without the debugging feature.

### Start the program on Mac or Linux

When you have installed the .Net Core, navigate with the terminal to the path of your project, enter in the directory MightyFileManager and type **dotnet run**.

## Commands list

0. Close the program
1. Download a file
2. Delete a file
3. Encrypt a file using Rijndael algorithm
4. Decrypt a file Rijndael algorithm
5. Delete all files in a directory
6. Encrypt all files in a folder and its sub folders and delete the old ones using Rijndael algorithm **(USE IT WISELY, THIS IS DANGEROUS)**
7. Decrypt all files in a folder and its sub folders and delete the old ones using Rijndael algorithm **(USE IT WISELY, THIS IS DANGEROUS)**
8. Encrypt a file in a folder using AES algorithm
9. Decrypt a file in a folder using AES algorithm
10. Encrypt all files in a folder and its sub folders and delete the old ones using AES algorithm **(USE IT WISELY, THIS IS DANGEROUS)**
11. Decrypt all files in a folder and its sub folders and delete the old ones using AES algorithm **(USE IT WISELY, THIS IS DANGEROUS)**

## Built with

* [.Net Core 2.1](https://dotnet.microsoft.com/download)
* [Visual Studio](https://visualstudio.microsoft.com/)
* [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)