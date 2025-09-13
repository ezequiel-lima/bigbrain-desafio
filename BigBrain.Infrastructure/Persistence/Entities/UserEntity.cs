using BigBrain.Core.Dtos;

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

        public static explicit operator UserEntity(GraphUserDto graphUserDto)
        {
            return new UserEntity
            {
                Id = graphUserDto.Id!.Value,
                BusinessPhones = graphUserDto.BusinessPhones,
                DisplayName = graphUserDto.DisplayName,
                GivenName = graphUserDto.GivenName,
                JobTitle = graphUserDto.JobTitle,
                Mail = graphUserDto.Mail,
                MobilePhone = graphUserDto.MobilePhone,
                OfficeLocation = graphUserDto.OfficeLocation,
                PreferredLanguage = graphUserDto.PreferredLanguage,
                Surname = graphUserDto.Surname,
                UserPrincipalName = graphUserDto.UserPrincipalName,
            };
        }
    }
}
