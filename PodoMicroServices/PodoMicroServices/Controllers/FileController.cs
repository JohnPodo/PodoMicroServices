using Microsoft.AspNetCore.Mvc;
using PodoMicroServices.Controllers.BaseClass;
using PodoMicroServices.Dto.FileDto;
using PodoMicroServices.Models;
using PodoMicroServices.Models.LogModels;
using PodoMicroServices.Services.FileServices;
using PodoMicroServices.Services.LogServices;

namespace PodoMicroServices.Controllers
{
    public class FileController : SuperController
    {
        private readonly FileService _service;

        public FileController(LogService logService, FileService service) : base(logService)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<BaseDataResponse<List<Models.FileModels.File>>>> GetFilesPerAppId([FromHeader] int appId)
        {
            try
            {
                await LogRequest();
                var data = await _service.GetFiles(appId);
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

        [HttpGet("{folderName}")]
        public async Task<ActionResult<BaseDataResponse<List<Models.FileModels.File>>>> GetFilesPerAppId([FromHeader] int appId,string folderName)
        {
            try
            {
                await LogRequest();
                var data = await _service.GetFiles(appId, folderName);
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

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseDataResponse<Models.FileModels.File>>> GetFile([FromHeader] int appId,int id)
        {
            try
            {
                await LogRequest();
                var data = await _service.GetFile(id, appId);
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
        public async Task<ActionResult<BaseResponse>> AddFile([FromHeader] int appId,[FromBody]FileDto dto)
        {
            try
            {
                await LogRequest();
                var response = new BaseResponse();
                var newFile = new Models.FileModels.File(dto, appId);
                if (appId <= 1) return BadRequest(new BaseResponse("Invalid App Id"));
                response = _service.ValidateFile(newFile);
                if (!response.Success) return BadRequest(response);
                response = await _service.AddFile(newFile);
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
        public async Task<ActionResult<BaseResponse>> DeleteFile(int id, [FromHeader] int appId)
        {
            try
            {
                await LogRequest();
                var response = new BaseResponse();
                response = await _service.DeleteFile(id, appId);
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
