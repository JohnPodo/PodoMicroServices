using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PodoMicroServices.Controllers.BaseClass;
using PodoMicroServices.Dto.SecretDto;
using PodoMicroServices.Models;
using PodoMicroServices.Models.LogModels;
using PodoMicroServices.Services;
using PodoMicroServices.Services.LogServices;
using PodoMicroServices.Services.SecretServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PodoMicroServices.Controllers
{
    public class SecretController : SuperController
    {
        private readonly SecretService _service;

        public SecretController(LogService logService, SecretService service, UserService userService) : base(logService, userService)
        {
            _service = service;
        }

        // GET: api/<SecretController>
        [HttpGet, Authorize]
        public async Task<ActionResult<BaseDataResponse<List<Models.SecretModels.Secret>>>> GetSecrets()
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found"));
                var data = await _service.GetSecrets(_app.Id);
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

        [HttpGet("{secretName}"), Authorize]
        public async Task<ActionResult<BaseDataResponse<List<Models.FileModels.File>>>> GetSecretByName( string secretName)
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found"));
                var data = await _service.GetSecret(_app.Id, secretName);
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
        public async Task<ActionResult<BaseDataResponse<List<Models.FileModels.File>>>> GetSecretById( int id)
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found"));
                var data = await _service.GetSecret( id, _app.Id);
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

        // POST api/<SecretController>
        [HttpPost, Authorize]
        public async Task<ActionResult<BaseResponse>> AddSecret([FromBody] SecretDto dto)
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found"));
                var response = new BaseResponse();
                var newSecret = new Models.SecretModels.Secret(dto, _app); 
                response = _service.ValidateSecret(newSecret);
                if (!response.Success) return BadRequest(response);
                response = await _service.AddSecret(newSecret);
                if (!response.Success) return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught with message : {ex.Message}", Severity.Fatal);
                return StatusCode(500);
            }
        }

        // PUT api/<SecretController>/5
        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult<BaseResponse>> UpdateSecret(int id, [FromBody] SecretDto dto)
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found"));
                var response = new BaseResponse();
                var newSecret = new Models.SecretModels.Secret(dto, _app); 
                response = _service.ValidateSecret(newSecret);
                if (!response.Success) return BadRequest(response);
                response = await _service.UpdateSecret(id,newSecret);
                if (!response.Success) return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught with message : {ex.Message}", Severity.Fatal);
                return StatusCode(500);
            }
        }

        // DELETE api/<SecretController>/5
        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult<BaseResponse>> DeleteSecret(int id)
        {
            try
            {
                await LogRequest();
                if (_app is null) return BadRequest(new BaseResponse("No App Found"));
                var response = await _service.DeleteSecret(id, _app.Id);
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
