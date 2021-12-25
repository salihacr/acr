using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acr.Core.Result
{
    public enum StatusType
    {
        Success = 200,
        Created = 201,
        BadRequest = 400,
        NotAuthorize = 401,
        AccessDenied = 403,
        NotFound = 404,
        InternalServerError = 500,
        Failed = 800,
    }
    public class DataResult<T> : TaskResult
    {
        public T Data { get; set; }
        public static DataResult<T> Successful(T data = default(T), string message = MessageSuccess)
        {
            return new DataResult<T> { Data = data, Status = StatusType.Success, Message = message, Success = true };
        }
    }
    public class ValidationResult : TaskResult
    {

    }
    public class TaskResult
    {
        public const string MessageSuccess = "Operation is successful";
        public const string MessageBadRequest = "Invalid parameter !! Please Check paramaters";
        public const string MessageInternalServer = "Internal Server error occured";
        public const string MessageNotFound = "Not Found";
        public const string MessageNotAuthorize = "Not Authorized";
        public const string MessageNotSupported = "Method is not supported";
        public const string MessageFailed = "Operation is failed";
        public const string MessageAuthorityNotFound = "Authority not found";
        public const string MessageAccessDenied = "Access Denied";



        [DefaultValue(false)]
        public bool Success { get; set; }
        public StatusType Status { get; set; }
        public string Message { get; set; }

        public static TaskResult Successful(string message = MessageSuccess)
        {
            return new TaskResult { Success = true, Status = StatusType.Success, Message = message };
        }

        public static TaskResult BadRequest(string message = MessageBadRequest)
        {
            return new TaskResult { Status = StatusType.BadRequest, Message = message };
        }
        public static TaskResult NotFound(string message = MessageNotFound)
        {
            return new TaskResult { Status = StatusType.NotFound, Message = message };
        }
        public static TaskResult NotAuthorize(string message = MessageNotAuthorize)
        {
            return new TaskResult { Status = StatusType.NotAuthorize, Message = message };
        }
        public static TaskResult InternalServerError(string message = MessageInternalServer)
        {
            return new TaskResult { Status = StatusType.InternalServerError, Message = message };
        }
        public static TaskResult Fail(string message = MessageFailed)
        {
            return new TaskResult { Status = StatusType.Failed, Message = message };
        }
    }
}