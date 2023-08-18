using DataAccess.Contexts;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utility;

namespace DataAccess
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            _db.Database.EnsureCreated();

            //run migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }
            //check to see if one table has data.  If so, do not insert any data
            if (_db.Posts.Any())
            {
                return; //DB has been seeded
            }

            //create roles if they are not created
            //SD is a “Static Details” class we will create in Utility to hold constant strings for Roles

            _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();

            //Create at least one "Super Admin" or “Admin”.  Repeat process for other users you want to seed

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "sond@metropolia.fi",
                Email = "sond@metropolia.fi",
                FirstName = "Son",
                LastName = "Dang",
                PhoneNumber = "123456",
                StreetAddress = "Street",
                State = "Uusimaa",
                PostalCode = "02400",
                City = "Kirkkonummi"
            }, "123456aA@").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => string.Compare(u.Email, "sond@metropolia.fi") == 0);

            _userManager.AddToRoleAsync(user, SD.AdminRole).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "john@metropolia.fi",
                Email = "john@metropolia.fi",
                FirstName = "John",
                LastName = "Smith",
                PhoneNumber = "9876543",
                StreetAddress = "Street",
                State = "Uusimaa",
                PostalCode = "02400",
                City = "Kirkkonummi"
            }, "123456aA@").GetAwaiter().GetResult();

            ApplicationUser normalUser = _db.ApplicationUsers.FirstOrDefault(u => string.Compare(u.Email, "john@metropolia.fi") == 0);

            _userManager.AddToRoleAsync(normalUser, SD.CustomerRole).GetAwaiter().GetResult();

            //Now continue with your tables

            var Statuses = new List<Status>
            {
                new Status {Name = SD.PostStatusAvailable},
                new Status {Name = SD.PostStatusReserve},
                new Status {Name = SD.PostStatusSold},
            };

            foreach (var c in Statuses)
            {
                _db.Status.Add(c);
            }
            _db.SaveChanges();

            Status Available = _db.Status.FirstOrDefault(u => string.Compare(u.Name, SD.PostStatusAvailable) == 0);
            Status Reserve = _db.Status.FirstOrDefault(u => string.Compare(u.Name, SD.PostStatusReserve) == 0);
            Status Sold = _db.Status.FirstOrDefault(u => string.Compare(u.Name, SD.PostStatusSold) == 0);


            var Tags = new List<Tag>
            {

            new Tag { TagName = "Phone" },
            new Tag { TagName = "Computer" },
            new Tag { TagName = "Television" },
            };

            foreach (var c in Tags)
            {
                _db.Tags.Add(c);
            }
            _db.SaveChanges();

            Tag Phone = _db.Tags.FirstOrDefault(u => string.Compare(u.TagName, "Phone") == 0);
            Tag Computer = _db.Tags.FirstOrDefault(u => string.Compare(u.TagName, "Computer") == 0);

            List<Tag> FirstList = new();
            FirstList.Add(Phone);
            List<Tag> SecondList = new();
            SecondList.Add(Phone);
            SecondList.Add(Computer);


            

            var Posts = new List<Post>
            {
                new Post
                 {
                     Title = "Iphone 14", Content = "unopened", AuthorId = normalUser.Id,
                     CreatedDateTime = DateTime.UtcNow, Price = 1200.00, StatusId = Available.Id, Tags = FirstList
                 },
                 new Post
                 {
                     Title = "Iphone 13", Content = "used", AuthorId = user.Id, CreatedDateTime = DateTime.UtcNow,
                     Price = 1000.00, StatusId = Reserve.Id, Tags = SecondList
                 },
                 new Post
                 {
                     Title = "Iphone 12", Content = "broken", AuthorId = normalUser.Id,
                     CreatedDateTime = DateTime.UtcNow, Price = 200.00, StatusId = Sold.Id
                 },
            };

            foreach (var m in Posts)
            {
                _db.Posts.Add(m);
            }
            _db.SaveChanges();

            Post Iphone12 = _db.Posts.FirstOrDefault(u => string.Compare(u.Title, "Iphone 12") == 0);
            Post Iphone13 = _db.Posts.FirstOrDefault(u => string.Compare(u.Title, "Iphone 13") == 0);
            Post Iphone14 = _db.Posts.FirstOrDefault(u => string.Compare(u.Title, "Iphone 14") == 0);

            var Attachments = new List<Attachment>
            {
                new Attachment{ MediaLink = "/images/iphone12.jpeg", Name = "iphone 12", PostId = Iphone12.Id},
                new Attachment{ MediaLink = "/images/iphone13.jpeg", Name = "iphone 13", PostId = Iphone13.Id},
                new Attachment{ MediaLink = "/images/iphone14.jpeg", Name = "iphone 14", PostId = Iphone14.Id},
            };

            foreach (var m in Attachments)
            {
                _db.Attachments.Add(m);
            }
            _db.SaveChanges();
        }

    }

}


