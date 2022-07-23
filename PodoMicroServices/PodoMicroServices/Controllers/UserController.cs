using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PodoMicroServices.Controllers.BaseClass;
using PodoMicroServices.Dto;
using PodoMicroServices.Models;
using PodoMicroServices.Models.LogModels;
using PodoMicroServices.Services;
using PodoMicroServices.Services.LogServices;

namespace PodoMicroServices.Controllers
{
    public class UserController : SuperController
    {

        public UserController(LogService logService, UserService service) : base(logService, service)
        {
        }

        [HttpGet, Authorize(Roles = "Boss")]
        public async Task<ActionResult<BaseDataResponse<List<User>>>> GetUsers()
        {
            try
            {
                await LogRequest();
                var response = await _userService.GetUsers(); 
                if (!response.Success) return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught with message : {ex.Message}", Severity.Fatal);
                return StatusCode(500);
            }
        }

        [HttpGet, Authorize(Roles = "Boss")]
        public async Task<ActionResult<BaseDataResponse<List<User>>>> GetUnacceptedUsers()
        {
            try
            {
                await LogRequest();
                var response = await _userService.GetUnacceptedUsers();
                if (!response.Success) return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught with message : {ex.Message}", Severity.Fatal);
                return StatusCode(500);
            }
        }

        [HttpPut("{id}"), Authorize(Roles = "Boss")]
        public async Task<ActionResult<BaseResponse>> HandleUser([FromBody] bool accept,int id)
        {
            try
            {
                await LogRequest();
                var response = await _userService.HandleUser(id, accept);
                if (!response.Success) return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught with message : {ex.Message}", Severity.Fatal);
                return StatusCode(500);
            }
        }

        [HttpPut, Authorize]
        public async Task<ActionResult<BaseResponse>> ChangePassword([FromBody] string newPassord)
        {
            try
            {
                await LogRequest();
                if(_userService._LoggedInUser is null) return BadRequest(new BaseResponse("No User Logged In"));
                var response = await _userService.ChangePassword(_userService._LoggedInUser.Id, newPassord);
                if (!response.Success) return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught with message : {ex.Message}", Severity.Fatal);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Register(RegisterDto dto)
        {
            try
            {
                await LogRequest();
                var response = _userService.ValidateNewUser(dto);
                if (!response.Success) return BadRequest(response);
                response = await _userService.Register(dto);
                if (!response.Success) return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught with message : {ex.Message}", Severity.Fatal);
                return StatusCode(500);
            }
        }

        [HttpPost, Authorize(Roles = "Boss")]
        public async Task<ActionResult<BaseResponse>> RegisterAdmin(RegisterDto dto)
        {
            try
            {
                await LogRequest();
                var response = _userService.ValidateNewUser(dto);
                if (!response.Success) return BadRequest(response);
                response = await _userService.Register(dto);
                if (!response.Success) return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught with message : {ex.Message}", Severity.Fatal);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseDataResponse<string>>> Login(UserDto dto)
        {
            try
            {
                await LogRequest();
                var response = await _userService.Login(dto);
                if (!response.Success) return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                await WriteToLog($"Exception caught with message : {ex.Message}", Severity.Fatal);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}"), Authorize(Roles = "Boss")]
        public async Task<ActionResult<BaseResponse>> DeleteUser(int id)
        {
            try
            {
                await LogRequest(); 
                var response = new BaseResponse();
                response = await _userService.DeleteUser(id);
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
