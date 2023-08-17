using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TradeHub.Pages.PostPage;

public class Upsert : PageModel
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

        
    [BindProperty]
    public Post ObjPost { get; set; }
    public Attachment ObjAttachment { get; set; }
    public string TagInput { get; set; }

    public IEnumerable<SelectListItem> StatusList { get; set; }
    public IEnumerable<SelectListItem> TagList { get; set; }

    public Upsert(UnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
        ObjPost = new Post();
        ObjAttachment = new Attachment();
        StatusList = new List<SelectListItem>();	
        TagList = new List<SelectListItem>();
           
    }

    public IActionResult OnGet(Guid? id)
    {
        ObjPost = new Post();
        StatusList = _unitOfWork.Status.GetAll()
            .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }
            );
        TagList = _unitOfWork.Tag.GetAll()
            .Select(m => new SelectListItem
                {
                    Text = m.TagName,
                    Value = m.Id.ToString()
                }
            );

        if (id == null || id == Guid.Empty) //create mode
        {
            return Page();
        }

        //edit mode

        if (id != Guid.Empty)  //retreive product from DB
        {
            ObjPost = _unitOfWork.Post.GetById(id);
        }

        if (ObjPost == null) //maybe nothing returned
        {
            return NotFound();
        }

        return Page();

    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        // Create tags
        if (!string.IsNullOrWhiteSpace(TagInput))
        {
            string[] tagNames = TagInput.Split(',');
            foreach (string tagName in tagNames)
            {
                Tag tag = new Tag { TagName = tagName.Trim() };
                _unitOfWork.Tag.Add(tag);
                _unitOfWork.Commit();
            }
        }
        
        // Upload attachment if provided
        if (ObjAttachment.MediaLink != null)
        {
            
        }

        return RedirectToPage("Index");
			
    }

}