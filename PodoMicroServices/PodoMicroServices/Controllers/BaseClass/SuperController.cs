using Microsoft.AspNetCore.Mvc;
using PodoMicroServices.Models.LogModels;
using PodoMicroServices.Services.LogServices;

namespace PodoMicroServices.Controllers.BaseClass
{
    [Route("api/[controller]/[action]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    public class SuperController : ControllerBase
    {

        protected readonly LogService _logService;
        public SuperController(LogService service)
        { 
            _logService = service;
            _logService.Initialize(this.GetType().Name, Guid.NewGuid());
        }
        protected async Task WriteToLog(string message, Severity severity)
        {
            _ = await _logService.WriteToLog(message, severity);
        }

        protected async Task LogRequest()
        {
            await WriteToLog($"Received request from {HttpContext.Request.Host.Value}", Severity.Information);
            await WriteToLog($"In Path {HttpContext.Request.Path}", Severity.Information);
        }
    }
}
