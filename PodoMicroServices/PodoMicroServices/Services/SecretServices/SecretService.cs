using Microsoft.EntityFrameworkCore;
using PodoMicroServices.Common;
using PodoMicroServices.DAL;

namespace PodoMicroServices.Services.SecretServices
{
    public class SecretService
    {
        private readonly PodoMicroServiceContext _context;

        public SecretService(PodoMicroServiceContext context)
        {
            _context = context;
        }

        public async Task<BaseDataResponse<Models.SecretModels.Secret>> GetSecret(int id, int appId)
        {
            if (id == 0) return new BaseDataResponse<Models.SecretModels.Secret>("Invalid Id");
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Secrets is null) throw new Exception("Secrets Db Set is null");
            var desiredSecret = await _context.Secrets.Where(f => f.Id == id).Where(f => f.App != null && f.App.Id == appId).AsNoTracking().FirstOrDefaultAsync();
            if (desiredSecret is null) return new BaseDataResponse<Models.SecretModels.Secret>("No Picture Found");
            return new BaseDataResponse<Models.SecretModels.Secret>(desiredSecret);
        }

        public async Task<BaseDataResponse<List<Models.SecretModels.Secret>>> GetSecrets(int appId)
        {
            if (appId == 0) return new BaseDataResponse<List<Models.SecretModels.Secret>>("Invalid appId");
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Secrets is null) throw new Exception("Secrets Db Set is null");
            var desiredFiles = await _context.Secrets.Where(f => f.App != null && f.App.Id == appId).AsNoTracking().ToListAsync();
            if (desiredFiles is null) return new BaseDataResponse<List<Models.SecretModels.Secret>>("No Pictures Found");
            return new BaseDataResponse<List<Models.SecretModels.Secret>>(desiredFiles);
        }

        public async Task<BaseDataResponse<Models.SecretModels.Secret>> GetSecret(int appId, string secretName)
        {
            if (appId == 0) return new BaseDataResponse<Models.SecretModels.Secret>("Invalid appId");
            if (string.IsNullOrEmpty(secretName)) return new BaseDataResponse<Models.SecretModels.Secret>("Invalid folderName");
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Secrets is null) throw new Exception("Secrets Db Set is null");
            var desiredFiles = await _context.Secrets.Where(f => f.App != null && f.App.Id == appId).Where(f => f.Name == secretName).AsNoTracking().FirstOrDefaultAsync();
            if (desiredFiles is null) return new BaseDataResponse<Models.SecretModels.Secret>("No Pictures Found");
            return new BaseDataResponse<Models.SecretModels.Secret>(desiredFiles);
        }

        public async Task<BaseResponse> AddSecret(Models.SecretModels.Secret newSecret)
        {
            if (newSecret is null) return new BaseResponse("Invalid data");
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Secrets is null) throw new Exception("Secrets Db Set is null");
            var desiredSecret = await _context.Secrets.Where(f => f.Name == newSecret.Name && f.App != null && f.App == newSecret.App).AsNoTracking().FirstOrDefaultAsync();
            if (desiredSecret is not null) return new BaseResponse("Secret with that name already exists");
            await _context.Secrets.AddAsync(newSecret);
            await _context.SaveChangesAsync();
            return new BaseResponse();
        }

        public async Task<BaseResponse> UpdateSecret(int id, Models.SecretModels.Secret newSecret)
        {
            if (newSecret is null) return new BaseResponse("Invalid data");
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Secrets is null) throw new Exception("Secrets Db Set is null");
            var desiredSecret = await _context.Secrets.Where(f => f.Id == id && f.App != null && f.App == newSecret.App).FirstOrDefaultAsync();
            if (desiredSecret is null) return new BaseResponse("Secret with that id doesn't exists");
            desiredSecret.ExpiresIn = newSecret.ExpiresIn;
            desiredSecret.Value = newSecret.Value;
            desiredSecret.CreatedAt = newSecret.CreatedAt;
            desiredSecret.Name = newSecret.Name;
            await _context.SaveChangesAsync();
            return new BaseResponse();
        }

        public async Task<BaseResponse> DeleteSecret(int id, int appId)
        {
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Secrets is null) throw new Exception("Secrets Db Set is null");
            var desiredSecret = await _context.Secrets.Where(f => f.Id == id && f.App != null && f.App.Id == appId).FirstOrDefaultAsync();
            if (desiredSecret is null) return new BaseResponse("Secret with that id doesn't exists");
            _context.Secrets.Remove(desiredSecret);
            await _context.SaveChangesAsync();
            return new BaseResponse();
        }
        public BaseResponse ValidateSecret(Models.SecretModels.Secret newSecret)
        {
            if (string.IsNullOrEmpty(newSecret.Name) || string.IsNullOrEmpty(newSecret.Value)) return new BaseResponse("Name and Value Fields are Required");
            return new BaseResponse();
        }

        internal async Task<BaseResponse> DeleteSecret()
        {
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Secrets is null) throw new Exception("Secrets Db Set is null");
            var desiredSecret = await _context.Secrets.Where(s=>s.ExpiresIn != null).ToListAsync();
            if (desiredSecret is null) return new BaseResponse("No Secrets Found");
            bool saveChanges = false;
            desiredSecret.ForEach(f =>
            {
                if (f.ExpiresIn > DateTime.Now.AddDays(-1))
                {
                    _context.Secrets.Remove(f);
                    saveChanges = true;
                }
            });
            if (saveChanges)
                await _context.SaveChangesAsync();
            return new BaseResponse();
        }
    }
}
