using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TradeHub.Pages.PostPage;

public class UpsertModel : PageModel
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

        
    [BindProperty]
    public Post ObjPost { get; set; }
    public Attachment ObjAttachment { get; set; }
    public string TagInput { get; set; }

    public IEnumerable<SelectListItem> StatusList { get; set; }
    public IEnumerable<SelectListItem> TagList { get; set; }

    public UpsertModel(UnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
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

    public IActionResult OnPost(Guid? id)
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
        
        //// Upload attachment if provided
        //if (ObjAttachment.MediaLink != null)
        //{
            
        //}

        //return RedirectToPage("Index");


        string webRootPath = _webHostEnvironment.WebRootPath;
        var files = HttpContext.Request.Form.Files;

        //if the product is new (create)

        if (id == null || id == Guid.Empty)
        {
            //was there even an image uploaded?

            if (files.Count > 0)
            {
                //create a unique identifier for image name
                string fileName = Guid.NewGuid().ToString();

                //create variable to hold a path to images\products
                var uploads = Path.Combine(webRootPath, @"images/products/"); // for Mac user

                //get and preserve the extension type 

                var extension = Path.GetExtension(files[0].FileName);

                // create the full upload path 

                var fullPath = uploads + fileName + extension;

                //stream the binary write to the server

                using var fileStream = System.IO.File.Create(fullPath);
                files[0].CopyTo(fileStream);

                //add this new Product internal model

                _unitOfWork.Post.Add(ObjPost);
                TempData["success"] = "Post added Successfully";

                //associate the actual URL path and save to DB URLImage

                Post NewPost = _unitOfWork.Post.GetByValue(ObjPost.Title);
                ObjAttachment = new Attachment();
                ObjAttachment.MediaLink = @"\images\products\" + fileName + extension;
                ObjAttachment.Name = fileName;
                ObjAttachment.PostId = NewPost.Id;
                _unitOfWork.Attachment.Add(ObjAttachment);
                
            }
            

        }

        //the item exists, so we're updating it

        else
        {
            //get the product again from the DB because
            //binding is on, and we need to process the
            //physical image separately from the binded
            //property holding URL string

            var objProductFromDb = _unitOfWork.Post.Get(p => p.Id == ObjPost.Id);
            //was there even an image uploaded?

            //if (files.Count > 0)
            //{
            //    //create a unique identifier for image name
            //    string fileName = Guid.NewGuid().ToString();

            //    //create variable to hold a path to images\products
            //    var uploads = Path.Combine(webRootPath, @"images\products\");

            //    //get and preserve the extension type 

            //    var extension = Path.GetExtension(files[0].FileName);

            //    //if the product stored in DB has image path

            //    if (objProductFromDb.ImageUrl != null)
            //    {
            //        var imagePath =
            //            Path.Combine(webRootPath, ObjPost.ImageUrl.TrimStart('\\'));

            //        //if the image exists physically

            //        if (System.IO.File.Exists(imagePath))
            //        {
            //            System.IO.File.Delete(imagePath);
            //        }
            //    }

            //    // create the full upload path 

            //    var fullPath = uploads + fileName + extension;

            //    //stream the binary write to the server

            //    using var fileStream = System.IO.File.Create(fullPath);
            //    files[0].CopyTo(fileStream);

            //    //associate the actual URL path and save to DB URLImage

            //    ObjProduct.ImageUrl = @"\images\products\" + fileName + extension;
            //}
            //else
            //{
            //    //we're trying to add image for 1st time
            //    //to to the product we are updating
            //    objProductFromDb.ImageUrl = ObjPost.ImageUrl;
            //}
            //update the existing product
            _unitOfWork.Post.Update(ObjPost);
            TempData["success"] = "Product updated Successfully";
        }
        //Save Changes to Database
        _unitOfWork.Commit();

        //redirect to the Products Page
        return RedirectToPage("./Index");

    }

}