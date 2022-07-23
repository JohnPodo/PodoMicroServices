using Microsoft.EntityFrameworkCore;
using PodoMicroServices.DAL;
using PodoMicroServices.Models;

namespace PodoMicroServices.Services.FileServices
{
    public class FileService
    {
        private readonly PodoMicroServiceContext _context;

        public FileService(PodoMicroServiceContext context)
        {
            _context = context;
        }

        public async Task<BaseDataResponse<Models.FileModels.File>> GetFile(int id, int appId)
        {
            if (id == 0) return new BaseDataResponse<Models.FileModels.File>("Invalid Id");
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Files is null) throw new Exception("Files Db Set is null");
            var desiredPic = await _context.Files.Where(f => f.Id == id).Where(f => f.App != null && f.App.Id == appId).AsNoTracking().FirstOrDefaultAsync();
            if (desiredPic is null) return new BaseDataResponse<Models.FileModels.File>("No Picture Found");
            return new BaseDataResponse<Models.FileModels.File>(desiredPic);
        }
        public async Task<BaseDataResponse<List<Models.FileModels.File>>> GetFiles(int appId)
        {
            if (appId == 0) return new BaseDataResponse<List<Models.FileModels.File>>("Invalid appId");
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Files is null) throw new Exception("Files Db Set is null");
            var desiredFiles = await _context.Files.Where(f => f.App != null && f.App.Id == appId).AsNoTracking().ToListAsync();
            if (desiredFiles is null) return new BaseDataResponse<List<Models.FileModels.File>>("No Pictures Found");
            return new BaseDataResponse<List<Models.FileModels.File>>(desiredFiles);
        }

        public async Task<BaseDataResponse<List<Models.FileModels.File>>> GetFiles(int appId, string folderName)
        {
            if (appId == 0) return new BaseDataResponse<List<Models.FileModels.File>>("Invalid appId");
            if (string.IsNullOrEmpty(folderName)) return new BaseDataResponse<List<Models.FileModels.File>>("Invalid folderName");
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Files is null) throw new Exception("Files Db Set is null");
            var desiredFiles = await _context.Files.Where(f => f.App != null && f.App.Id == appId).Where(f => f.Folder == folderName).AsNoTracking().ToListAsync();
            if (desiredFiles is null) return new BaseDataResponse<List<Models.FileModels.File>>("No Pictures Found");
            return new BaseDataResponse<List<Models.FileModels.File>>(desiredFiles);
        }

        public BaseResponse ValidateFile(Models.FileModels.File newFile)
        {
            if (string.IsNullOrEmpty(newFile.Name) || string.IsNullOrEmpty(newFile.Content)) return new BaseResponse("Name and Content Field are Required");
            if (!IsBase64String(newFile.Content)) return new BaseResponse("Invalid Content");
            return new BaseResponse();
        }

        public async Task<BaseResponse> AddFile(Models.FileModels.File newFile)
        {
            if (newFile is null) return new BaseResponse("Invalid data");
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Files is null) throw new Exception("Files Db Set is null");
            var desiredFile = await _context.Files.Where(f => f.Name == newFile.Name && f.Folder == newFile.Folder && f.App == newFile.App).AsNoTracking().FirstOrDefaultAsync();
            if (desiredFile is not null) return new BaseResponse("File with that name already exists");
            await _context.Files.AddAsync(newFile);
            await _context.SaveChangesAsync();
            return new BaseResponse();
        }

        public bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }

        public async Task<BaseResponse> DeleteFile(int id, int appId)
        {
            if (id == 0) return new BaseResponse("Invalid id");
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Files is null) throw new Exception("Files Db Set is null");
            var desiredFile = await _context.Files.Where(f => f.Id == id).Where(f => f.App != null && f.App.Id == appId).FirstOrDefaultAsync();
            if (desiredFile is null) return new BaseResponse("No File found.");
            _context.Files.Remove(desiredFile);
            await _context.SaveChangesAsync();
            return new BaseResponse();
        }
    }
}
