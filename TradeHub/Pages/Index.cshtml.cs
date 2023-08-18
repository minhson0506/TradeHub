using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TradeHub.Pages
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
            ObjPostList = _unitOfWork.Post.GetAll();
            return Page();
        }
    }
}