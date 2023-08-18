using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Contexts;
using DataAccess.Models;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TradeHub.Pages
{
    public class CommentsModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;

        private readonly UserManager<IdentityUser> _userManager;

        public CommentsModel(UnitOfWork unitOfWork, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _userManager = userManager;
        }

        [BindProperty]
        public string Content { get; set; }

        public Guid PostId { get; set; }
        public string PostTitle { get; set; }
        public List<Comment> Comments { get; set; }

        public IActionResult OnGet(Guid postId)
        {
            // PostId = postId;
            var post = _db.Posts.Find(postId);
            if (post == null)
            {
                return NotFound();
            }
            PostTitle = post.Title;

            Comments = _db.Comments
                .Where(c => c.PostId == postId)
                .OrderByDescending(c => c.CreatedDateTime)
                .ToList();

            return Page();
        }

        public IActionResult OnPost(Guid postId)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                userId = ApplicationUser.ReferenceEquals(userId, null) ? "501d9c0f-7dc9-4b9e-a27f-90742e992d58" : userId;
                var comment = new Comment
                {

                    Content = Content,
                    PostId = postId,
                    UserId = userId,
                    CreatedDateTime = DateTime.UtcNow
                };

                _unitOfWork.Comment.Add(comment);
                _db.SaveChanges();

                return RedirectToPage("Comments", new { postId });
            }

            // Handle invalid model state
            // You can add error messages to ModelState if needed

            return RedirectToPage("Comments", new { postId });
        }
    }
}