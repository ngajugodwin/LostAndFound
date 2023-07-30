using LostAndFound_API.Domain.Models;
using LostAndFound_API.Resources.Item;

namespace LostAndFound_API.Extensions
{
    public static class EntityExtension
    {
        public static string GetPreferredContactInformation(this Item item)
        {
           if (item != null)
            {
                switch (item.ModeOfContact)
                {
                    case ModeOfContact.Email:
                        return item.ReportedByUser != null ? item.ReportedByUser.Email : string.Empty;
                case ModeOfContact.Phone:
                        return item.ReportedByUser != null ? item.ReportedByUser.PhoneNumber : string.Empty;
                    default:
                        return item.ReportedByUser != null ? item.ReportedByUser.Email : string.Empty;
                }
            }

            return string.Empty;
        }

        public static List<ItemCommentResource> GetUserComments(this ICollection<ItemComment> itemComments)
        {
            if(itemComments.Count() > 0)
            {
                var comments = new List<ItemCommentResource>();

                foreach (var comment in itemComments)
                {
                    comments.Add(new ItemCommentResource
                    {
                        Comment = comment.Comment,
                        CommentedBy = comment.CommentByUser != null ? comment.CommentByUser.Email : string.Empty,
                        CreatedAt = comment.CreatedAt
                    });
                }

                return comments;
            }

            return new List<ItemCommentResource>();
        }
    }
}
