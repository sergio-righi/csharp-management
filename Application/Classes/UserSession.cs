using Application.Extensions;
using Application.Helpers;
using Domain.Entity.Specific;
using System;
using System.Web;
using Tool.Utilities;

namespace Application.Classes
{
    public class UserSession
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public ERole[] Roles { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public EPerson TypeId { get; set; }
        public DateTime? Birthday { get; set; }

        public UserSession()
        {
        }

        public UserSession(SUserPerson userPerson)
        {
            this.Id = userPerson.Id;
            this.Name = userPerson.Name;
            this.Email = userPerson.Email;
            this.TypeId = userPerson.TypeId;
            this.Photo = userPerson.FullPhotoPath;
            this.Birthday = userPerson.Birthday;
        }

        public static bool Opened()
        {
            return AppContextHelper.Current.Session.Get<UserSession>(Tool.Configurations.AppConfig.Session.User) != null;
        }

        public static UserSession Get()
        {
            return AppContextHelper.Current.Session.Get<UserSession>(Tool.Configurations.AppConfig.Session.User);
        }
    }
}