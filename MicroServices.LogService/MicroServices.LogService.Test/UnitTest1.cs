using MicroServices.LogService.Client;
using MicroServices.LogService.Common;

namespace MicroServices.LogService.Test
{
    public class UnitTest1
    {
        private LogClient Client { get; set; }
        public UnitTest1()
        {
            Client = new LogClient("http://localhost/LogService/api/Log", 1001, "LogTests");
        }

        private async Task<bool> GetLogs()
        {
            try
            { 
                var result = await Client.GetLogs();
                Assert.NotNull(result);
                if(result is null) return false;
                Assert.True(result.Success);
                Assert.True(string.IsNullOrEmpty(result.Message));
                Assert.NotNull(result.Data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> GetLogs(Severity severity)
        {
            try
            {
                var result = await Client.GetLogs(severity);
                Assert.NotNull(result);
                if (result is null) return false;
                Assert.True(result.Success);
                Assert.True(string.IsNullOrEmpty(result.Message));
                Assert.NotNull(result.Data); 
                if (result.Data is null) return false;
                if (result.Data.Count != 0)
                { 
                    Assert.True(await GetLogs(result.Data.Last().GroupSession));
                    Assert.True(await DeleteLog(result.Data.Last().Id));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> GetLogs(Guid session)
        {
            try
            {
                var result = await Client.GetLogs(session);
                Assert.NotNull(result);
                if (result is null) return false;
                Assert.True(result.Success);
                Assert.True(string.IsNullOrEmpty(result.Message));
                Assert.NotNull(result.Data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> WriteToLog(string message,Severity severity)
        {
            try
            {
                var result = await Client.WriteToLog(message,severity);
                Assert.NotNull(result);
                if (result is null) return false;
                Assert.True(result.Success);
                Assert.True(string.IsNullOrEmpty(result.Message)); 
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> DeleteLog(int id)
        {
            try
            {
                var result = await Client.DeleteLog(id);
                Assert.NotNull(result);
                if (result is null) return false;
                Assert.True(result.Success);
                Assert.True(string.IsNullOrEmpty(result.Message));
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Fact]
        public async Task Test()
        {
            Assert.True(await WriteToLog("Test",Severity.Information));
            Assert.True(await GetLogs()); 
            Assert.True(await GetLogs(Severity.Information)); 
        }
    }
}