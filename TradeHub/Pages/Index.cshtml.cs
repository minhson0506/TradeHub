using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TradeHub.Pages;

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
        ObjPostList = _unitOfWork.Post.GetAll();
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

