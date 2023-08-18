using System.Collections;
using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TradeHub.Pages;

public class IndexModel : PageModel
{
    private readonly UnitOfWork _unitOfWork;
    public IEnumerable<Post> ObjPostList;
    public IEnumerable<Attachment> ObjAttachmentList;
    public IndexModel(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ObjPostList = Enumerable.Empty<Post>();
        ObjAttachmentList = Enumerable.Empty<Attachment>();
    }

    public IActionResult OnGet()
    {
        ObjPostList = _unitOfWork.Post.GetAll();
        //ObjPostList = await Task.Run(() => _unitOfWork.Post.GetAllAsync());

        //ObjAttachmentList = await Task.Run(() =>  _unitOfWork.Attachment.GetAllAsync());
        //foreach (var obj in ObjPostList)
        //{
        //    var Files =  ObjAttachmentList.Where(attachment => attachment.PostId == obj.Id);
        //    foreach (var link in Files)
        //    {
        //        obj.Files.Add(link.MediaLink);
        //    }
        //}
        return Page();
        Console.WriteLine(ObjPostList);
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

