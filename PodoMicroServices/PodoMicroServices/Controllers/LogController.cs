using Microsoft.AspNetCore.Mvc;
using PodoMicroServices.Controllers.BaseClass;
using PodoMicroServices.Dto.LogDto;
using PodoMicroServices.Models;
using PodoMicroServices.Models.LogModels;
using PodoMicroServices.Services.LogServices;

namespace PodoMicroServices.Controllers
{
    public class LogController : SuperController
    {
        public LogController(LogService service) : base(service)
        {
        }


        [HttpGet]
        public async Task<ActionResult<BaseDataResponse<List<Log>>>> GetLogs([FromHeader] int appId)
        {
            try
            {
                await LogRequest();
                var data = await _logService.GetLogs(appId);
                if (data is null) return StatusCode(500);
                if (!data.Success) return BadRequest(data);
                return Ok(data);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught with message : {ex.Message}", Severity.Fatal);
                return StatusCode(500);
            }

        }

        [HttpGet("{session}")]
        public async Task<ActionResult<BaseDataResponse<List<Log>>>> GetLogsPerSession(Guid session, [FromHeader] int appId)
        {
            try
            {
                await LogRequest();
                var data = await _logService.GetLogs(session, appId);
                if (data is null) return StatusCode(500);
                if (!data.Success) return BadRequest(data);
                return Ok(data);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught with message : {ex.Message}", Severity.Fatal);
                return StatusCode(500);
            }
        }

        [HttpGet("{severity}")]
        public async Task<ActionResult<BaseDataResponse<List<Log>>>> GetLogsPerSeverity(Severity severity, [FromHeader] int appId)
        {
            try
            {
                await LogRequest();
                var data = await _logService.GetLogs(appId, severity);
                if (data is null) return StatusCode(500);
                if (!data.Success) return BadRequest(data);
                return Ok(data);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught with message : {ex.Message}", Severity.Fatal);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> WriteLog([FromHeader] int appId,[FromBody] LogDto dto)
        {
            try
            {
                await LogRequest();
                if(appId <= 1) return BadRequest(new BaseResponse("Invalid App Id"));
                var response = new BaseResponse();
                (response.Success, response.Message) = _logService.ValidateLogDto(dto);
                if (!response.Success) return BadRequest(response);
                (response.Success, response.Message) = await _logService.WriteToLog(new Log(dto,appId));
                if (!response.Success) return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught with message : {ex.Message}", Severity.Fatal);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse>> DeleteLog(int id,[FromHeader] int appId)
        {
            try
            {
                await LogRequest();
                var response = new BaseResponse();
                (response.Success, response.Message) = await _logService.DeleteLog(id, appId);
                if (!response.Success) return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught with message : {ex.Message}", Severity.Fatal);
                return StatusCode(500);
            }
        }
    }
}
