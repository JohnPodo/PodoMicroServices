using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PodoMicroServices.Controllers.BaseClass;
using PodoMicroServices.Dto.EmailDto;
using PodoMicroServices.Models;
using PodoMicroServices.Models.LogModels;
using PodoMicroServices.Services;
using PodoMicroServices.Services.EmailServices;
using PodoMicroServices.Services.LogServices;

namespace PodoMicroServices.Controllers
{
    public class EmailController : SuperController
    {
        private readonly EmailService _service;

        public EmailController(LogService logService, EmailService service,UserService userService) : base(logService, userService)
        {
            _service = service;
        }

        [HttpPost,Authorize]
        public async Task<ActionResult<BaseResponse>> DeleteLog(EmailDto dto)
        {
            try
            {
                await LogRequest();
                var response = new BaseResponse();
                response = _service.ValidateDto(dto);
                if(!response.Success)
                    return BadRequest(response);
                response = await _service.SendEmail(dto);
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
