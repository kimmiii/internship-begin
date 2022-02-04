using System.Collections.Generic;
using EventAPI.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Services
{
    public interface IFileStorageService
    {
        Task<List<FileStorage>> GetFilesAsync(string path);
        Task<FileStream> GetFileAsync(string path, string fileName);
        Task<FileStorage> UploadFilesAsync(string path, Stream streamObject, string objectContentType);
        Task DeleteFileAsync(string relativePath);
    }
    public class FileStorageService : IFileStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly string _rootPath;

        public FileStorageService(
            IConfiguration configuration)
        {
            _configuration = configuration;
            _rootPath = _configuration.GetSection("Services")
                .GetSection("FileStorage")
                .GetSection("Local")["Path"];
        }

        public async Task<List<FileStorage>> GetFilesAsync(string path)
        {
            var fullpath = _rootPath + path;

            var storageFiles = new List<FileStorage>();

            if (Directory.Exists(fullpath))
            {
                var filePaths = Directory.GetFiles(fullpath).ToList();
                filePaths.ForEach(f => storageFiles.Add(
                    new FileStorage()
                    {
                        FileName = Path.GetFileName(f),
                        RelativePath = path +Path.GetFileName(f),
                        FullPath = f,
                        DateModified = File.GetLastWriteTime(f),
                        ContentType = Path.GetExtension(f)
                    }
                ));
            }
            return await Task.FromResult(storageFiles);
        }

        public async Task<FileStream> GetFileAsync(string path, string fileName)
        {
            var directoryPath = _rootPath + path;

            var fullPath = Path.Combine(directoryPath, fileName);

            return await Task.FromResult(new FileStream(fullPath, FileMode.Open, FileAccess.Read));
        }

        public async Task<FileStorage> UploadFilesAsync(string path, Stream file, string contentype)
        {
            var fullpath = _rootPath + path;
            var directory = Path.GetDirectoryName(fullpath);

            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            file.Position = 0;
            await using var fileStream = new FileStream(fullpath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            file.Close();

            return new FileStorage()
            {
                FileName = Path.GetFileName(fullpath),
                RelativePath = path + Path.GetFileName(fullpath),
                FullPath = fullpath,
                ContentType = contentype,
                DateModified = File.GetLastWriteTime(fullpath)
            };
        }

        public Task DeleteFileAsync(string relativePath)
        {
            string fullPath = _rootPath + relativePath;

            if (!File.Exists(fullPath))
            {
                return Task.CompletedTask;
            }

            File.Delete(fullPath);

            return Task.CompletedTask;
        }
    }
}
