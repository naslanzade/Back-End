using System.Diagnostics;

namespace OneSoundApp.Helpers
{
    public static class FileExtentions
    {
        public static bool CheckFileType(this IFormFile file,string pattern) 
        {
            return file.ContentType.Contains(pattern);
        }

        public static bool CheckFileSize(this IFormFile file,long size)
        {
            return file.Length/1024 > size;
        }


        public static async Task SaveFileAsync(this IFormFile file,string fileName,string basePath,string folder)
        {
           
            string path = Path.Combine(basePath, folder, fileName);


            using (FileStream stream = new(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }          

        }


        public static void DeleteFile(string path)
        {

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

        }

        public static string GetFilePath(string root, string folder, string file)
        {
            return Path.Combine(root, folder, file);
        }


    }
}
