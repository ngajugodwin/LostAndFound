using LostAndFound_API.Domain.Models;

namespace LostAndFound_API.Domain.Services.Communication
{
    public class ItemResponse : BaseResponse<Item>
    {
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="item">Item response.</param>
        /// <returns>Response.</returns>
        public ItemResponse(Item item) : base(item)
        { }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public ItemResponse(string message) : base(message)
        { }
    }
}
