using Microsoft.Graph.Models;
using System.Text.Json;

namespace BigBrain.Core.Dtos
{
    public class GraphUserDto
    {
        public Guid? Id { get; set; }
        public string? BusinessPhones { get; set; }
        public string? DisplayName { get; set; }
        public string? GivenName { get; set; }
        public string? JobTitle { get; set; }
        public string? Mail { get; set; }
        public string? MobilePhone { get; set; }
        public string? OfficeLocation { get; set; }
        public string? PreferredLanguage { get; set; }
        public string? Surname { get; set; }
        public string? UserPrincipalName { get; set; }

        public static explicit operator GraphUserDto(User user)
        {
            return new GraphUserDto
            {
                Id = Guid.TryParse(user.Id, out var guid) ? guid : null,

                BusinessPhones = user.BusinessPhones?.Any() == true
                    ? JsonSerializer.Serialize(user.BusinessPhones)
                    : null,

                DisplayName = user.DisplayName,
                GivenName = user.GivenName,
                JobTitle = user.JobTitle,
                Mail = user.Mail,
                MobilePhone = user.MobilePhone,
                OfficeLocation = user.OfficeLocation,
                PreferredLanguage = user.PreferredLanguage,
                Surname = user.Surname,
                UserPrincipalName = user.UserPrincipalName
            };
        }
    }
}
