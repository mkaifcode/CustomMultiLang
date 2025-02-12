using Microsoft.AspNetCore.Mvc;
using MultiLangApi.Services.Localization;
using Newtonsoft.Json;
using System.Net;

namespace MultiLangApi.Helpers.Response
{
    public class ResponseHelper
    {
        private readonly JsonLocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResponseHelper(JsonLocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
        {
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }      
        

        public CommonResponse CreateResponse(string responseKey, object responseData, int statusCode, bool isError = false)
        {
            var language = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? "en";
            string localizedMessage = _localizationService.GetLocalizedString(responseKey, language);

            return new CommonResponse
            {
                RequestId = Guid.NewGuid(),
                Data = responseData,
                Message = localizedMessage,
                IsError = isError,
                Status = ResponseStatusCode(statusCode)
            };
        }

        public HttpStatusCode ResponseStatusCode(int statusCode) 
        => statusCode switch
        {
            200 => HttpStatusCode.OK,
            202 => HttpStatusCode.Accepted,
            204 => HttpStatusCode.NoContent,
            401 => HttpStatusCode.Unauthorized,
            403 => HttpStatusCode.Forbidden,
            404 => HttpStatusCode.NotFound,
            409 => HttpStatusCode.Conflict,
            500 => HttpStatusCode.InternalServerError,
            _ => HttpStatusCode.BadRequest
        };

        public IActionResult ResponseWrapper(CommonResponse response)
        {
            return response.Status switch
            {
                HttpStatusCode.OK => new OkObjectResult(response),
                HttpStatusCode.Accepted => new AcceptedResult(),
                HttpStatusCode.NoContent => new NoContentResult(),
                HttpStatusCode.Unauthorized => new UnauthorizedResult(),
                HttpStatusCode.Forbidden => new ForbidResult(),
                HttpStatusCode.NotFound => new NotFoundObjectResult(response),
                HttpStatusCode.Conflict => new ConflictObjectResult(response),
                HttpStatusCode.InternalServerError => new ObjectResult(response) { StatusCode = 500 },
                _ => new BadRequestObjectResult(response)
            };
        }

        public class CommonResponse
        {
            public Guid RequestId { get; set; }
            public DateTime RequestTime { get; set; } = DateTime.UtcNow;
            public HttpStatusCode Status { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
            public bool IsError { get; set; }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
    }
}
