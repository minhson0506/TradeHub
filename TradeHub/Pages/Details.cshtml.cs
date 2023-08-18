using DataAccess.Contexts;
using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using System.Security.Claims;

namespace TradeHub.Pages
{

    public class DetailsModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;

        private readonly UserManager<IdentityUser> _userManager;
        public Post PostProduct { get; set; }

        public DetailsModel(UnitOfWork unitOfWork, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
              PostProduct = new Post();
            _unitOfWork = unitOfWork;
            _db = db;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult OnGet(Guid postId)
        {
			PostProduct = _unitOfWork.Post.Get(p => p.Id.Equals(postId));
			return Page();
		}
        public IActionResult OnPost(Post PostProduct)
        {

          return Page();
        }

 
    }
}
