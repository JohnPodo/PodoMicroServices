using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PodoMicroServices.Controllers.BaseClass;
using PodoMicroServices.Dto.LogDto;
using PodoMicroServices.Models;
using PodoMicroServices.Models.LogModels;
using PodoMicroServices.Services;
using PodoMicroServices.Services.LogServices;

namespace PodoMicroServices.Controllers
{
    public class LogController : SuperController
    {
        public LogController(LogService service,UserService userService) : base(service, userService)
        {
        }


        [HttpGet, Authorize]
        public async Task<ActionResult<BaseDataResponse<List<Log>>>> GetLogs()
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found"));
                var data = await _logService.GetLogs(_app.Id);
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

        [HttpGet("{session}"), Authorize]
        public async Task<ActionResult<BaseDataResponse<List<Log>>>> GetLogsPerSession(Guid session)
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found"));
                var data = await _logService.GetLogs(session, _app.Id);
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

        [HttpGet("{severity}"), Authorize]
        public async Task<ActionResult<BaseDataResponse<List<Log>>>> GetLogsPerSeverity(Severity severity)
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found"));
                var data = await _logService.GetLogs(_app.Id, severity);
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

        [HttpPost, Authorize]
        public async Task<ActionResult<BaseResponse>> WriteLog([FromBody] LogDto dto)
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found")); 
                var response = new BaseResponse();
                (response.Success, response.Message) = _logService.ValidateLogDto(dto);
                if (!response.Success) return BadRequest(response);
                (response.Success, response.Message) = await _logService.WriteToLog(new Log(dto,_app));
                if (!response.Success) return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught with message : {ex.Message}", Severity.Fatal);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult<BaseResponse>> DeleteLog(int id)
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found"));
                var response = new BaseResponse();
                (response.Success, response.Message) = await _logService.DeleteLog(id, _app.Id);
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
