using BigBrain.Core.Models;
using Microsoft.Graph.Models;
using System.Text.Json;

namespace BigBrain.Infrastructure.Persistence.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
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

        public static explicit operator UserEntity(UserModel user)
        {
            return new UserEntity
            {
                Id = user.Id,
                BusinessPhones = user.BusinessPhones,
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
