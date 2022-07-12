using MicroServices.LogService.Common;
using MicroServices.LogService.Models;
using MicroServices.LogService.Services;
using Microsoft.AspNetCore.Mvc;


namespace MicroServices.LogService.Controllers
{
    [Route("api/[controller]/[action]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly LogFetcher _fetcher;
        private readonly Services.LogService _service;
        private readonly LogCleaner _cleaner;

        public LogController(LogFetcher fetcher, Services.LogService service,LogCleaner cleaner)
        {
            _fetcher = fetcher;
            _service = service;
            _cleaner = cleaner;
            _service.Initialize("LogController", Guid.NewGuid());
        }

        [HttpGet]
        public async Task<ActionResult<BaseDataResponse<List<Log>>>> GetLogs([FromHeader] int appId)
        {
            await LogRequest();
            var data = await _fetcher.GetLogs(appId);
            if (data is null) return StatusCode(500);
            if (!data.Success) return BadRequest(data);
            return Ok(data);
        }

        [HttpGet("{session}")]
        public async Task<ActionResult<BaseDataResponse<List<Log>>>> GetLogsPerSession(Guid session, [FromHeader] int appId)
        {
            await LogRequest();
            var data = await _fetcher.GetLogs(session, appId);
            if (data is null) return StatusCode(500);
            if (!data.Success) return BadRequest(data);
            return Ok(data);
        }

        [HttpGet("{severity}")]
        public async Task<ActionResult<BaseDataResponse<List<Log>>>> GetLogsPerSeverity(Severity severity, [FromHeader] int appId)
        {
            await LogRequest();
            var data = await _fetcher.GetLogs(appId, severity);
            if (data is null) return StatusCode(500);
            if (!data.Success) return BadRequest(data);
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> WriteLog([FromBody]LogDto dto)
        {
            await LogRequest();
            var response = new BaseResponse();
            (response.Success, response.Message) = _service.ValidateLogDto(dto); 
            if (!response.Success) return BadRequest(response);
            (response.Success, response.Message) = await _service.WriteToLog(new Log(dto));
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse>> DeleteLog(int id)
        {
            await LogRequest();
            var response = new BaseResponse();
            (response.Success, response.Message) = await _service.DeleteLog(id); 
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        private async Task LogRequest()
        {
            await WriteToLog($"Received request from {HttpContext.Request.Host.Value}", Severity.Information);
            await WriteToLog($"In Path {HttpContext.Request.Path}", Severity.Information);
        }

        #region
        private async Task WriteToLog(string message, Severity severity)
        {
            _ = await _service.WriteToLog(message, severity);
        }
        #endregion
    }
}
