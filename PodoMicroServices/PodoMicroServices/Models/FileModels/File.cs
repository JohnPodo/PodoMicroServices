﻿using PodoMicroServices.Common.Dto.FileDto;

namespace PodoMicroServices.Models.FileModels
{
    public class File : BaseModel
    {
        public string Folder { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Alt { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;

        public File()
        {

        }

        public File(FileDto dto, App app)
        {
            Folder = dto.Folder;
            Content = dto.Content;
            Alt = dto.Alt;
            Created = dto.Created;
            Name = dto.Name;
            App = app;
        }
    }
}
