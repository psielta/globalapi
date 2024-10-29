using System;

namespace GlobalErpData.Models
{
    public abstract class ApiResponse
    {
        public int StatusCode { get; protected set; }
        public string Message { get; protected set; }

        protected ApiResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }

    public class Success : ApiResponse
    {
        public Success() : base(200, "Success") { }
        public Success(string message) : base(200, message) { }
    }

    public class Created : ApiResponse
    {
        public Created() : base(201, "Created") { }
        public Created(string message) : base(201, message) { }
    }

    public class BadRequest : ApiResponse
    {
        public BadRequest() : base(400, "Bad Request") { }
        public BadRequest(string message) : base(400, message) { }
    }

    public class NotFound : ApiResponse
    {
        public NotFound() : base(404, "Not Found") { }
        public NotFound(string message) : base(404, message) { }
    }

    public class Forbidden : ApiResponse
    {
        public Forbidden() : base(403, "Forbidden") { }
        public Forbidden(string message) : base(403, message) { }
    }

    public class InternalServerError : ApiResponse
    {
        public InternalServerError() : base(500, "Internal Server Error") { }
        public InternalServerError(string message) : base(500, message) { }
    }
}
