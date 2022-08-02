using Newtonsoft.Json;
using PodoMicroServices.Common;
using PodoMicroServices.Common.Dto.EmailDto;
using PodoMicroServices.Common.Dto.FileDto;
using PodoMicroServices.Common.Dto.LogDto;
using PodoMicroServices.Common.Dto.SecretDto;
using PodoMicroServices.Consumer.Models;
using RestSharp;

namespace PodoMicroServices.Consumer
{
    public class PodoConsumer
    {
        private readonly string _endpoint;
        private string token = string.Empty;

        public PodoConsumer(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint)) throw new ArgumentNullException("Endpoint cannot be null");
            _endpoint = endpoint;
        }

        public async Task<BaseResponse> Login(string username, string password, int appId)
        {
            try
            {
                if (string.IsNullOrEmpty(username)) return new BaseResponse("Username is required");
                if (string.IsNullOrEmpty(password)) return new BaseResponse("Password is required");
                if (appId == 0) return new BaseResponse("No AppId with 0");
                var body = new
                {
                    username = username,
                    password = password,
                    appId = appId
                };
                var response = await GetResponse("/api/User/Login", Method.Post, body, null);
                if (response == null)
                    return new BaseResponse("Request Failed");
                if (response.IsSuccessful)
                {
                    BaseDataResponse<string>? result;
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        result = JsonConvert.DeserializeObject<BaseDataResponse<string>>(response.Content);
                        if (result is not null)
                            if (result.Success)
                                if (!string.IsNullOrEmpty(result.Data))
                                {
                                    token = result.Data;
                                    return new BaseResponse();
                                }
                                else
                                    return new BaseResponse("Request Failed");
                            else return new BaseResponse(result.Message);
                        return new BaseResponse("Request Failed");
                    }

                }
                return new BaseResponse("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseResponse(ex.Message);
            }

        }

        public async Task<BaseResponse> SendEmail(EmailDto dto)
        {
            try
            {
                if (dto is null) return new BaseResponse("dto is null");
                if (string.IsNullOrEmpty(token)) return new BaseResponse("Login First");
                var response = await GetResponse("/api/Email/SendEmail", Method.Post, dto, null);
                if (response == null)
                    return new BaseResponse("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseResponse>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseResponse("Request Failed");
                    }
                }
                return new BaseResponse("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseResponse(ex.Message);
            }
        }

        public async Task<BaseDataResponse<List<PodoFile>>> GetFiles()
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return new BaseDataResponse<List<PodoFile>>("Login First");
                var response = await GetResponse("/api/File/GetFilesPerAppId", Method.Get, null, null);
                if (response == null)
                    return new BaseDataResponse<List<PodoFile>>("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseDataResponse<List<PodoFile>>>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseDataResponse<List<PodoFile>>("Request Failed");
                    }
                }
                return new BaseDataResponse<List<PodoFile>>("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseDataResponse<List<PodoFile>>(ex.Message);
            }
        }

        public async Task<BaseDataResponse<List<PodoFile>>> GetFiles(string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return new BaseDataResponse<List<PodoFile>>("Login First");
                if (string.IsNullOrEmpty(folderName)) return new BaseDataResponse<List<PodoFile>>("folderName is required");
                var response = await GetResponse($"/api/File/GetFilesPerAppId/{folderName}", Method.Get, null, null);
                if (response == null)
                    return new BaseDataResponse<List<PodoFile>>("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseDataResponse<List<PodoFile>>>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseDataResponse<List<PodoFile>>("Request Failed");
                    }
                }
                return new BaseDataResponse<List<PodoFile>>("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseDataResponse<List<PodoFile>>(ex.Message);
            }
        }
        public async Task<BaseDataResponse<PodoFile>> GetFile(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return new BaseDataResponse<PodoFile>("Login First");
                if (id == 0) return new BaseDataResponse<PodoFile>("id is required");
                var response = await GetResponse($"/api/File/GetFile/{id}", Method.Get, null, null);
                if (response == null)
                    return new BaseDataResponse<PodoFile>("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseDataResponse<PodoFile>>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseDataResponse<PodoFile>("Request Failed");
                    }
                }
                return new BaseDataResponse<PodoFile>("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseDataResponse<PodoFile>(ex.Message);
            }
        }

        public async Task<BaseDataResponse<List<PodoLog>>> GetLogs()
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return new BaseDataResponse<List<PodoLog>>("Login First");
                var response = await GetResponse($"/api/Log/GetLogs", Method.Get, null, null);
                if (response == null)
                    return new BaseDataResponse<List<PodoLog>>("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseDataResponse<List<PodoLog>>>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseDataResponse<List<PodoLog>>("Request Failed");
                    }
                }
                return new BaseDataResponse<List<PodoLog>>("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseDataResponse<List<PodoLog>>(ex.Message);
            }
        }

        public async Task<BaseDataResponse<List<PodoLog>>> GetLogs(Guid session)
        {
            try
            {
                if (session == Guid.Empty) return new BaseDataResponse<List<PodoLog>>("session is required");
                if (string.IsNullOrEmpty(token)) return new BaseDataResponse<List<PodoLog>>("Login First");
                var response = await GetResponse($"/api/Log/GetLogsPerSession/{session}", Method.Get, null, null);
                if (response == null)
                    return new BaseDataResponse<List<PodoLog>>("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseDataResponse<List<PodoLog>>>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseDataResponse<List<PodoLog>>("Request Failed");
                    }
                }
                return new BaseDataResponse<List<PodoLog>>("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseDataResponse<List<PodoLog>>(ex.Message);
            }
        }

        public async Task<BaseDataResponse<List<PodoLog>>> GetLogs(Severity severity)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return new BaseDataResponse<List<PodoLog>>("Login First");
                var response = await GetResponse($"/api/Log/GetLogsPerSeverity/{severity}", Method.Get, null, null);
                if (response == null)
                    return new BaseDataResponse<List<PodoLog>>("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseDataResponse<List<PodoLog>>>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseDataResponse<List<PodoLog>>("Request Failed");
                    }
                }
                return new BaseDataResponse<List<PodoLog>>("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseDataResponse<List<PodoLog>>(ex.Message);
            }
        }

        public async Task<BaseDataResponse<List<PodoSecret>>> GetSecrets()
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return new BaseDataResponse<List<PodoSecret>>("Login First");
                var response = await GetResponse($"/api/Secret/GetSecrets", Method.Get, null, null);
                if (response == null)
                    return new BaseDataResponse<List<PodoSecret>>("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseDataResponse<List<PodoSecret>>>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseDataResponse<List<PodoSecret>>("Request Failed");
                    }
                }
                return new BaseDataResponse<List<PodoSecret>>("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseDataResponse<List<PodoSecret>>(ex.Message);
            }
        }

        public async Task<BaseDataResponse<PodoSecret>> GetSecret(string secretName)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return new BaseDataResponse<PodoSecret>("Login First");
                if (string.IsNullOrEmpty(secretName)) return new BaseDataResponse<PodoSecret>("secretName is required");
                var response = await GetResponse($"/api/Secret/GetSecretByName/{secretName}", Method.Get, null, null);
                if (response == null)
                    return new BaseDataResponse<PodoSecret>("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseDataResponse<PodoSecret>>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseDataResponse<PodoSecret>("Request Failed");
                    }
                }
                return new BaseDataResponse<PodoSecret>("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseDataResponse<PodoSecret>(ex.Message);
            }
        }

        public async Task<BaseDataResponse<PodoSecret>> GetSecret(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return new BaseDataResponse<PodoSecret>("Login First");
                if (id == 0) return new BaseDataResponse<PodoSecret>("id is required");
                var response = await GetResponse($"/api/Secret/GetSecretById/{id}", Method.Get, null, null);
                if (response == null)
                    return new BaseDataResponse<PodoSecret>("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseDataResponse<PodoSecret>>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseDataResponse<PodoSecret>("Request Failed");
                    }
                }
                return new BaseDataResponse<PodoSecret>("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseDataResponse<PodoSecret>(ex.Message);
            }
        }
        public async Task<BaseResponse> AddFile(FileDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return new BaseResponse("Login First");
                if (dto is null) return new BaseResponse("dto is required");
                var response = await GetResponse($"/api/File/AddFile", Method.Post, dto, null);
                if (response == null)
                    return new BaseResponse("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseResponse>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseResponse("Request Failed");
                    }
                }
                return new BaseResponse("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseResponse(ex.Message);
            }
        }

        public async Task<BaseResponse> DeleteFile(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return new BaseResponse("Login First");
                if (id == 0) return new BaseResponse("id is required");
                var response = await GetResponse($"/api/File/DeleteFile/{id}", Method.Delete, null, null);
                if (response == null)
                    return new BaseResponse("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseResponse>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseResponse("Request Failed");
                    }
                }
                return new BaseResponse("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseResponse(ex.Message);
            }
        }

        public async Task<BaseResponse> WriteLog(LogDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return new BaseResponse("Login First");
                if (dto is null) return new BaseResponse("dto is required");
                var response = await GetResponse($"/api/Log/WriteLog", Method.Post, dto, null);
                if (response == null)
                    return new BaseResponse("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseResponse>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseResponse("Request Failed");
                    }
                }
                return new BaseResponse("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseResponse(ex.Message);
            }
        }

        public async Task<BaseResponse> DeleteLog(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return new BaseResponse("Login First");
                if (id == 0) return new BaseResponse("id is required");
                var response = await GetResponse($"/api/Log/DeleteLog/{id}", Method.Delete, null, null);
                if (response == null)
                    return new BaseResponse("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseResponse>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseResponse("Request Failed");
                    }
                }
                return new BaseResponse("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseResponse(ex.Message);
            }
        }

        public async Task<BaseResponse> AddSecret(SecretDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return new BaseResponse("Login First");
                if (dto is null) return new BaseResponse("dto is required");
                var response = await GetResponse($"/api/Secret/AddSecret", Method.Post, dto, null);
                if (response == null)
                    return new BaseResponse("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseResponse>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseResponse("Request Failed");
                    }
                }
                return new BaseResponse("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseResponse(ex.Message);
            }
        }

        public async Task<BaseResponse> DeleteSecret(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return new BaseResponse("Login First"); 
                if (id == 0) return new BaseResponse("id is required"); 
                var response = await GetResponse($"/api/Secret/DeleteSecret/{id}", Method.Delete, null, null);
                if (response == null)
                    return new BaseResponse("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseResponse>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseResponse("Request Failed");
                    }
                }
                return new BaseResponse("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseResponse(ex.Message);
            }
        }

        public async Task<BaseResponse> UpdateSecret(int id, SecretDto newSecret)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return new BaseResponse("Login First");
                if (newSecret is null) return new BaseResponse("newSecret is required");
                if (id == 0) return new BaseResponse("id is required");
                var response = await GetResponse($"/api/Secret/UpdateSecret/{id}", Method.Put, newSecret, null);
                if (response == null)
                    return new BaseResponse("Request Failed");
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var result = JsonConvert.DeserializeObject<BaseResponse>(response.Content);
                        if (result is not null)
                            return result;
                        return new BaseResponse("Request Failed");
                    }
                }
                return new BaseResponse("Request Failed");
            }
            catch (Exception ex)
            {
                return new BaseResponse(ex.Message);
            }
        }
        private async Task<RestResponse> GetResponse(string extension, Method method, object? body, Dictionary<string, string>? customHeaders)
        {
            var client = new RestClient(_endpoint);
            var request = new RestRequest(extension, method);
            if (!string.IsNullOrEmpty(token))
                request.AddHeader("Authorization", $"bearer {token}");
            if (customHeaders is not null)
                foreach (var combo in customHeaders)
                {
                    request.AddHeader(combo.Key, combo.Value);
                }
            request.RequestFormat = DataFormat.Json;
            if (body is not null)
            {
                var jsonBody = JsonConvert.SerializeObject(body, Formatting.Indented);
                request.AddJsonBody(jsonBody);
            }
            return await client.ExecuteAsync(request);
        }
    }
}
