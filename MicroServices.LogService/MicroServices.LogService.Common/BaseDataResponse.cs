using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroServices.LogService.Common
{
    public class BaseDataResponse<T> : BaseResponse
    {
        public T? Data { get; set; }

        public BaseDataResponse()
        {

        }

        public BaseDataResponse(string message) : base(message)
        {
            Data = default(T);
        }

        public BaseDataResponse(T? data)
        {
            Data = data;
        }
    }
}
