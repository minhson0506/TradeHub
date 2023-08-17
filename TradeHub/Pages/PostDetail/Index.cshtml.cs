using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TradeHub.Pages.PostDetail;

public class Index : PageModel
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    
    [BindProperty]public Post ObjPost { get; set; }
    public ApplicationUser? ObjApplicationUser { get; set; }
    
    public Index(UnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
        ObjPost = new Post();
    }
    
    public IActionResult OnGet(string? id)
    {
        /*if (id == null)
        {
            return Page();
        }*/

        ObjPost = _unitOfWork.Post
            .GetById(Guid.Parse("188b490a-f32c-497e-9aca-6a84b557208a"));
        
        if (ObjPost == null)
        {
            return NotFound();
        }
        
        ObjPost.PostAuthor = _unitOfWork.ApplicationUser
            .GetApplicationUserById(ObjPost.AuthorId);
        
        return Page();
    }
}