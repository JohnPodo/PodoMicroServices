using Microsoft.EntityFrameworkCore;
using PodoMicroServices.Models.LogModels;
using PodoMicroServices.Services.LogServices;

namespace PodoMicroServices.Services.SecretServices
{ 
    public class SecretCleaner : SuperCleaner
    { 
        private SecretService _secretService; 
        public SecretCleaner(IConfiguration config) : base(config)
        { 
            _secretService = new SecretService(new DAL.PodoMicroServiceContext(new DbContextOptionsBuilder().UseSqlServer(config["Base:ConnectionString"]).Options));
        } 

        protected override async Task LocalExecution()
        {
            var result = await _secretService.DeleteSecret();
            await WriteToLog($"Success : {result.Success}", result.Success ? Severity.Information : Severity.Error);
            await WriteToLog($"Messages : {result.Message}", string.IsNullOrEmpty(result.Message) ? Severity.Information : Severity.Error);
        }
    }
}
