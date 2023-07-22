namespace LostAndFound_API.Domain.Services.Communication
{
    public class BaseResponse<T>
    {
        public bool Success { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public T Resource { get; private set; } = default(T);
        public List<T> Resources { get; private set; } = new List<T>();

        protected BaseResponse(T resource)
        {
            Success = true;
            Message = string.Empty;
            Resource = resource;
        }

        protected BaseResponse(List<T> resources)
        {
            Success = true;
            Message = string.Empty;
            Resources = resources;
        }

        protected BaseResponse(string message)
        {
            Success = false;
            Message = message;
            Resource = default(T);
        }
    }
}
