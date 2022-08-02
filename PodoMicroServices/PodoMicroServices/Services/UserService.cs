using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PodoMicroServices.Common;
using PodoMicroServices.DAL;
using PodoMicroServices.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace PodoMicroServices.Services
{
    public class UserService
    {
        private readonly PodoMicroServiceContext? _context;
        private readonly IConfiguration _config;
        public User? _LoggedInUser;
        public App? _App;

        public UserService(PodoMicroServiceContext context, IConfiguration config, IHttpContextAccessor accesor)
        {
            _context = context;
            _config = config;
            if (accesor is not null)
            {
                if (accesor.HttpContext is not null)
                {
                    if (_context is not null)
                    {
                        if (_context.Users is not null)
                        {
                            var username = accesor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                            if (!string.IsNullOrEmpty(username))
                                _LoggedInUser = _context.Users.Where(s => s.Username == username).FirstOrDefault();
                        }
                        if (_context.Apps is not null)
                        {
                            var appId = accesor.HttpContext.User.FindFirstValue("AppId");
                            if (!string.IsNullOrEmpty(appId))
                                _App = _context.Apps.Where(s => s.Id == int.Parse(appId)).FirstOrDefault();
                        }
                    }
                }
            }
        }

        public BaseResponse ValidateNewUser(RegisterDto dto)
        {
            if (dto is null) return new BaseResponse("No Data Given");
            if (string.IsNullOrEmpty(dto.Username)) return new BaseResponse("Username is necessary");
            if (string.IsNullOrEmpty(dto.Password)) return new BaseResponse("Password is necessary");
            if (_context is null) return new BaseResponse("Could not Validate");
            if (_context.Users is null) return new BaseResponse("Could not Validate");
            if (_context.Users.Where(u => u.Username == dto.Username).Count() != 0) return new BaseResponse("Username exists already");
            return new BaseResponse();
        }

        public async Task<BaseResponse> Register(RegisterDto dto)
        {
            User newUser = new User();
            newUser.Username = dto.Username;
            (newUser.PasswordSalt, newUser.PasswordHash) = CreatePasswordHash(dto.Password);
            newUser.Role = "Noob";
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Users is null) throw new Exception("Users Db Set is null");
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return new BaseResponse();
        }

        public async Task<BaseResponse> RegisterAdmin(RegisterDto dto)
        {
            User newUser = new User();
            newUser.Username = dto.Username;
            (newUser.PasswordSalt, newUser.PasswordHash) = CreatePasswordHash(dto.Password);
            newUser.Role = "Boss";
            newUser.Accepted = true;
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Users is null) throw new Exception("Users Db Set is null");
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return new BaseResponse();
        }

        public async Task<BaseDataResponse<string>> Login(UserDto dto)
        {
            if (dto is null) return new BaseDataResponse<string>("Invalid Data");
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Users is null) throw new Exception("Users Db Set is null");
            var users = await _context.Users.Where(u => u.Username == dto.Username && u.Accepted).Include(u => u.Apps).AsNoTracking().ToListAsync();
            if (users.Count > 1) throw new Exception("Found more than one user with same credentials");
            var user = users.FirstOrDefault();
            if (user is null) return new BaseDataResponse<string>("No User Found");
            if (dto.AppId != 0)
                if (user.Apps.FirstOrDefault(a => a.Id == dto.AppId) == null) return new BaseDataResponse<string>("Invalid AppId");
            if (!VerifyPassword(dto.Password, user.PasswordSalt, user.PasswordHash)) return new BaseDataResponse<string>("Invalid Credentials");
            var token = CreateToken(user, dto.AppId);
            return new BaseDataResponse<string>() { Data = token };
        }

        private (byte[], byte[]) CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return (passwordSalt, passwordHash);
            }
        }

        private bool VerifyPassword(string password, byte[]? passwordSalt, byte[]? passwordHash)
        {
            if (passwordSalt is null || passwordHash is null) return false;
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user, int appId)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim("AppId",appId.ToString())
            };
            var secretKey = _config["Base:SecretTokenKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
           );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<BaseDataResponse<List<User>>> GetUsers()
        {
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Users is null) throw new Exception("Users Db Set is null");
            var users = await _context.Users.AsNoTracking().ToListAsync();
            return new BaseDataResponse<List<User>>(users);
        }

        public async Task<BaseDataResponse<List<User>>> GetUnacceptedUsers()
        {
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Users is null) throw new Exception("Users Db Set is null");
            var users = await _context.Users.Where(u => u.Accepted == false).AsNoTracking().ToListAsync();
            return new BaseDataResponse<List<User>>(users);
        }

        public async Task<BaseResponse> HandleUser(int id, bool accept)
        {
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Users is null) throw new Exception("Users Db Set is null");
            var user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user is null) return new BaseResponse("No User with that Id");
            if (!accept)
            {
                _context.Users.Remove(user);
            }
            else
            {
                user.Accepted = accept;
            }
            await _context.SaveChangesAsync();
            return new BaseResponse();
        }

        public async Task<BaseResponse> ChangePassword(int id, string newPassword)
        {
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Users is null) throw new Exception("Users Db Set is null");
            var user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user is null) return new BaseResponse("No User with that Id");
            (user.PasswordSalt, user.PasswordHash) = CreatePasswordHash(newPassword);
            await _context.SaveChangesAsync();
            return new BaseResponse();
        }

        public async Task<BaseResponse> DeleteUser(int id)
        {
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Users is null) throw new Exception("Users Db Set is null");
            var user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user is null) return new BaseResponse("No User with that Id");
            user.Apps.ForEach(async a =>
            {
                _context?.Logs?.RemoveRange(await _context.Logs.Where(l => l.App != null && l.App.Id == a.Id).ToArrayAsync());
                _context?.Files?.RemoveRange(await _context.Files.Where(l => l.App != null && l.App.Id == a.Id).ToArrayAsync());
                _context?.Secrets?.RemoveRange(await _context.Secrets.Where(l => l.App != null && l.App.Id == a.Id).ToArrayAsync());
                if (_context is null) throw new Exception("Database Context is null");
                await _context.SaveChangesAsync();
                _context.Apps?.Remove(a);
                await _context.SaveChangesAsync();
            });
            _context.Logs?.RemoveRange(await _context.Logs.Where(l => l.App == null).ToArrayAsync());
            _context.Files?.RemoveRange(await _context.Files.Where(l => l.App == null).ToArrayAsync());
            _context.Secrets?.RemoveRange(await _context.Secrets.Where(l => l.App == null).ToArrayAsync());
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return new BaseResponse();
        }

        public async Task<BaseResponse> RegisterApp(string appName, int userId)
        {
            if (string.IsNullOrEmpty(appName)) return new BaseResponse("App Name is required");
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Users is null) throw new Exception("Users Db Set is null");
            var user = await _context.Users.Where(s => s.Id == userId && s.Accepted).FirstOrDefaultAsync();
            if (user is null) return new BaseResponse("Invalid User");
            App newApp = new App()
            {
                Name = appName,
                User = user
            };
            if (_context.Apps is null) throw new Exception("Apps Db Set is null");
            await _context.Apps.AddAsync(newApp);
            await _context.SaveChangesAsync();
            return new BaseResponse();
        }

        public async Task<BaseResponse> DeleteApp(int appId, int userId)
        {
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Apps is null) throw new Exception("Apps Db Set is null");
            var wantedApp = await _context.Apps.Where(a => a.Id == appId && a.User != null && a.User.Id == userId).FirstOrDefaultAsync();
            if (wantedApp is null) return new BaseResponse("No app found to delete");
            _context.Apps.Remove(wantedApp);
            await _context.SaveChangesAsync();
            return new BaseResponse();
        }

        public async Task<BaseDataResponse<List<App>>> GetMyApps(int userId)
        {
            if (_context is null) throw new Exception("Database Context is null");
            if (_context.Apps is null) throw new Exception("Apps Db Set is null");
            var wantedApps = await _context.Apps.Where(a => a.User != null && a.User.Id == userId).AsNoTracking().ToListAsync();
            return new BaseDataResponse<List<App>>(wantedApps);
        }
    }
}
