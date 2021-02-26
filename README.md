# ImagesImporter4XamariniOS
A simple console application to perform mass import of images into the Xamarin.iOS project. Forked from `yanxiaodi/ImagesImporter4XamariniOS`.

## Usage
1. Generate all the @1x, @2x and @3x PNG images you wish to import using an appropriate graphics tools -- Inkscape can be run from the command line to generate the required images.
2. Make sure they use the naming convention `<name_of_image_set>@1x.png` etc.
3. Copy all the images from all the image sets into one folder and note its path `<image_folder_name>/*.png`
4. Build and publish the application at the Package Manager Console within Visual Studio e.g. `dotnet publish -c Release -r win10-x64` and copy the target directory to a directory near to your `<image_folder_name>` directory.
5. Run a command prompt and navigate to the directory containing the executable and run it.
6. When prompted enter the **RELATIVE** path of the directory containing all your images.
7. When prompted enter the name of the assets catalog folder minus its extension -- this can be found by navigating to your Xamarin.iOS project. The folder will be called `<name_of_catalog>.xcassets`.
8. Once complete the program will have created a folder called `<name_of_catalog>.xcassets` within which are individual folders containing each image set which you can copy straight into your `<name_of_catalog>.xcassets` folder in yoru Xamarin.iOS project code directory. You will also have a text file containing the lines that need adding to your Xamarin.iOS project csproj file. Make sure you reload the project afterwards.
