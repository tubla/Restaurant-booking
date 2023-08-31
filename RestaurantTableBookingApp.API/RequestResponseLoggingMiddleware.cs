using Serilog;

namespace RestaurantTableBookingApp.API
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Log.Information($"Request : {context.Request.Method} {context.Request.Path}");

            //copy the original response body stream
            var originalBodyStream = context.Response.Body;

            //create a new memory stream to capture the response
            using (var responseBody = new MemoryStream())
            {
                //Set the response body stream to the memory stream
                context.Response.Body = responseBody;

                //continue processing the request
                await _next(context);

                //log the response
                var response = await FormatResponse(context.Response);
                Log.Information($"Response: {response}");

                // Copy the captured response to the original response body stream
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return $"{response.StatusCode} : {text}";
        }
    }
}
