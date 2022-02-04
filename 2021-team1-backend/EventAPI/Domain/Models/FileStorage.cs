using System;

namespace EventAPI.Domain.Models
{
    public class FileStorage
    {
        public string FileName { get; set; }
        public string RelativePath { get; set; }
        public string FullPath { get; set; }
        public string ContentType { get; set; }
        public Uri Uri { get; set; }
        public DateTime DateModified { get; set; }
    }
}
