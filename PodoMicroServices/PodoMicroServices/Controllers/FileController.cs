using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PodoMicroServices.Common;
using PodoMicroServices.Common.Dto.FileDto;
using PodoMicroServices.Controllers.BaseClass;
using PodoMicroServices.Services;
using PodoMicroServices.Services.FileServices;
using PodoMicroServices.Services.LogServices;

namespace PodoMicroServices.Controllers
{
    public class FileController : SuperController
    {
        private readonly FileService _service;

        public FileController(LogService logService, FileService service, UserService userService) : base(logService, userService)
        {
            _service = service;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<BaseDataResponse<List<Models.FileModels.File>>>> GetFilesPerAppId()
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found"));
                var data = await _service.GetFiles(_app.Id);
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

        [HttpGet("{folderName}"), Authorize]
        public async Task<ActionResult<BaseDataResponse<List<Models.FileModels.File>>>> GetFilesPerAppId(string folderName)
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found"));
                var data = await _service.GetFiles(_app.Id, folderName);
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

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<BaseDataResponse<Models.FileModels.File>>> GetFile(int id)
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found"));
                var data = await _service.GetFile(id, _app.Id);
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
        public async Task<ActionResult<BaseResponse>> AddFile([FromBody] FileDto dto)
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found"));
                var response = new BaseResponse();
                var newFile = new Models.FileModels.File(dto, _app);
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

        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult<BaseResponse>> DeleteFile(int id)
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found"));
                var response = new BaseResponse();
                response = await _service.DeleteFile(id, _app.Id);
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
