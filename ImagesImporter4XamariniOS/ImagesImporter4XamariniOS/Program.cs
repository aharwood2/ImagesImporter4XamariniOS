using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ImagesImporter4XamariniOS
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World! Welcome to ImagesImporter4iOS. Please input the image folder path:");
                var path = Console.ReadLine();
                var sourceDirectory = GetDirectory(path);
                var images = GetImages(sourceDirectory);

                Console.WriteLine("Please input the Asset Catalog Name:");
                var assetCatalogName = Console.ReadLine();

                GenerateResources(assetCatalogName, sourceDirectory, images);
                Console.WriteLine($"Completed! Please copy the {assetCatalogName}.xcassets folder into your root folder of your iOS project, and copy the content of the csproject.txt into your *.csproj file.");
                Console.WriteLine("Press Enter to exit.");
                var enter = Console.ReadKey();
                if (enter.Key == ConsoleKey.Enter)
                {
                    Environment.Exit(0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }
            
        }

        private static void GenerateResources(string assetCatalogName, DirectoryInfo sourceDirectory, List<FileInfo> images)
        {
            var targetDirectory = sourceDirectory.CreateSubdirectory($"{assetCatalogName}.xcassets");
            StringBuilder sb = new StringBuilder();
            foreach (var fileInfo in images)
            {
                // Handle string of image
                var fileName = fileInfo.Name;
                var fileNameClip = fileName.Substring(0, fileName.IndexOf('@'));
                var fileSizeExt = fileName.Substring(fileName.IndexOf('@') + 1, 1);
                var dirName = $"{fileNameClip}.imageset";

                // Check if the directory already exists
                var imageFolderPath = Path.Combine(targetDirectory.FullName, dirName);
                DirectoryInfo imageFolder;
                var insertImage = false;
                if (Directory.Exists(imageFolderPath))
                {
                    imageFolder = GetDirectory(imageFolderPath);
                    insertImage = true;
                }
                else
                {
                    imageFolder = targetDirectory.CreateSubdirectory(dirName);
                }
                
                // Copy in image file
                fileInfo.CopyTo(Path.Combine(imageFolder.FullName, fileName));

                // Are we creating a new JSON or modifying an existing one
                string content;
                if (insertImage)
                {
                    // Read existing from directory
                    content = File.ReadAllText(Path.Combine(imageFolder.FullName, "Contents.json"));
                    
                }
                else
                {
                    // Read template from working directory
                    content = File.ReadAllText("Contents.json");
                }

                // Modify contents and write
                File.WriteAllText(Path.Combine(imageFolder.FullName, "Contents.json"),
                        content.Replace($"[[filename{fileSizeExt}]]", fileName));


                // Create csproj contents
                sb.Append(
                    $@"<ImageAsset Include=""{assetCatalogName}.xcassets\{fileName.Replace(fileInfo.Extension, "")}.imageset\Contents.json"">");
                sb.Append(Environment.NewLine);
                sb.Append(@"<Visible>false</Visible>");
                sb.Append(Environment.NewLine);
                sb.Append(@"</ImageAsset>");
                sb.Append(Environment.NewLine);
                sb.Append(
                    $@"<ImageAsset Include=""{assetCatalogName}.xcassets\{fileName.Replace(fileInfo.Extension, "")}.imageset\{fileName}"">");
                sb.Append(Environment.NewLine);
                sb.Append(@"<Visible>false</Visible>");
                sb.Append(Environment.NewLine);
                sb.Append(@"</ImageAsset>");
                sb.Append(Environment.NewLine);
            }

            File.WriteAllText(Path.Combine(targetDirectory.FullName, "csproject.txt"), sb.ToString());
        }

        private static DirectoryInfo GetDirectory(string path)
        {
            path = Path.GetFullPath(path);
            if (Directory.Exists(path)) return new DirectoryInfo(path);
            throw new Exception($"Path: {path} is invalid");
        }

        private static List<FileInfo> GetImages(DirectoryInfo directory)
        {
            var allFiles = directory.GetFiles();
            var images = allFiles.Where(x => x.Extension.ToLower() == ".jpg" || x.Extension.ToLower() == ".png").ToList();
            return images;
        }
    }
}
