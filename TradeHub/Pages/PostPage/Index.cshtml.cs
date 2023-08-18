using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TradeHub.Pages.PostPage
{
	public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        public IEnumerable<Post> ObjPostList;
        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            ObjPostList = new List<Post>();
        }

        public IActionResult OnGet()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ObjPostList = _unitOfWork.Post.GetAll().Where(obj => obj.AuthorId == claim.Value);
            
            return Page();
        }

        public IActionResult OnPostMarkSold(Guid id)
        {
            var post = _unitOfWork.Post.GetById(id);
            if (post != null)
            {
                // Update the status of the post to "Sold"
                post.Status.Name = "Sold";
                _unitOfWork.Post.Update(post);
                _unitOfWork.Commit();
            }

            return Page();
        }
    }
}
