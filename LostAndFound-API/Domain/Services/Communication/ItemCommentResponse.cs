using LostAndFound_API.Domain.Models;

namespace LostAndFound_API.Domain.Services.Communication
{
    public class ItemCommentResponse : BaseResponse<ItemComment>
    {
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="itemComment">Item comment response.</param>
        /// <returns>Response.</returns>
        public ItemCommentResponse(ItemComment itemComment) : base(itemComment)
        { }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public ItemCommentResponse(string message) : base(message)
        { }
    }
}
