using Microsoft.AspNetCore.Mvc;
using PodoMicroServices.Models;
using PodoMicroServices.Models.LogModels;
using PodoMicroServices.Services;
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
        protected readonly UserService _userService;
        protected readonly App? _app;

        public SuperController(LogService service,UserService userService)
        { 
            _logService = service;
            _userService = userService;
            _logService.Initialize(this.GetType().Name, Guid.NewGuid());
            _app = _userService._App;
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
