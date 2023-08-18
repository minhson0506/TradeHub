using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TradeHub.Pages.PostPage;

public class Delete : PageModel
{
    private readonly UnitOfWork _unitOfWork;

    [BindProperty]
    public Post ObjPost { get; set; }
    public Attachment ObjAttachment { get; set; }
    public IEnumerable<SelectListItem> StatusList { get; set; }

    public Delete(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult OnGet(Guid? id)
    {
        if (id == null)
        {
            // Handle the case where id is null
            return NotFound(); // or perform any other appropriate action
        }

        ObjPost = _unitOfWork.Post.GetById(id);

        if (ObjPost == null)
        {
            // Handle the case where ObjPost is null (post not found)
            return NotFound(); // or perform any other appropriate action
        }

        StatusList = _unitOfWork.Status.GetAll()
            .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

        return Page();
    }

    public IActionResult OnPost()
    {
        _unitOfWork.Post.Delete(ObjPost);
        TempData["success"] = "Product deleted Successfully";

        _unitOfWork.Commit();

        return RedirectToPage("./Index");
    }
}