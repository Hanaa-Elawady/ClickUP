using AspNetCore.Identity.Mongo.Model;

namespace ClickUp.Data.Entities.IdentityEntities
{
    public class ApplicationUser :MongoUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
