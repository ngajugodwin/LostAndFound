using LostAndFound_API.Domain.Models;

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
    }
}
