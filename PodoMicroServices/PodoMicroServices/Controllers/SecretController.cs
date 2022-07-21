using Microsoft.AspNetCore.Mvc;
using PodoMicroServices.Controllers.BaseClass;
using PodoMicroServices.Dto.SecretDto;
using PodoMicroServices.Models;
using PodoMicroServices.Models.LogModels;
using PodoMicroServices.Services.LogServices;
using PodoMicroServices.Services.SecretServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PodoMicroServices.Controllers
{
    public class SecretController : SuperController
    {
        private readonly SecretService _service;

        public SecretController(LogService logService, SecretService service) : base(logService)
        {
            _service = service;
        }

        // GET: api/<SecretController>
        [HttpGet]
        public async Task<ActionResult<BaseDataResponse<List<Models.SecretModels.Secret>>>> GetSecrets([FromHeader] int appId)
        {
            try
            {
                await LogRequest();
                var data = await _service.GetSecrets(appId);
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

        [HttpGet("{secretName}")]
        public async Task<ActionResult<BaseDataResponse<List<Models.FileModels.File>>>> GetSecretByName([FromHeader] int appId, string secretName)
        {
            try
            {
                await LogRequest();
                var data = await _service.GetSecret(appId, secretName);
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
        public async Task<ActionResult<BaseDataResponse<List<Models.FileModels.File>>>> GetSecretById([FromHeader] int appId, int id)
        {
            try
            {
                await LogRequest();
                var data = await _service.GetSecret( id, appId);
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
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddSecret([FromBody] SecretDto dto, [FromHeader] int appId)
        {
            try
            {
                await LogRequest();
                var response = new BaseResponse();
                var newSecret = new Models.SecretModels.Secret(dto, appId);
                if (appId <= 1) return BadRequest(new BaseResponse("Invalid App Id"));
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
        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse>> UpdateSecret(int id, [FromBody] SecretDto dto, [FromHeader] int appId)
        {
            try
            {
                await LogRequest();
                var response = new BaseResponse();
                var newSecret = new Models.SecretModels.Secret(dto, appId);
                if (appId <= 1) return BadRequest(new BaseResponse("Invalid App Id"));
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
        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse>> DeleteSecret([FromHeader] int appId,int id)
        {
            try
            {
                await LogRequest();  
                if (appId <= 1) return BadRequest(new BaseResponse("Invalid App Id"));  
                var response = await _service.DeleteSecret(id, appId);
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
