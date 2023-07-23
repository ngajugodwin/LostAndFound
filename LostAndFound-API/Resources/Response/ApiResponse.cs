namespace LostAndFound_API.Resources.Response
{
    public class ApiResponse
    {
        public string Message { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public object? Data { get; set; }


        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="response">Success response.</param>
        /// <returns>Response.</returns>
        public ApiResponse(string code, string message, object data)
        {
            this.Code = code;
            this.Message = message;
            this.Data = data;
        }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public ApiResponse(string code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}
