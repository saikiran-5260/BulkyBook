
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public IActionResult Index()
        {
            //IEnumerable<Product> objCoverTypeList = _unitOfWork.Product.GetAll();
            return View(/*objCoverTypeList*/);
        }
        //Get
        //public IActionResult Create()
        //{

        //    return View();
        //}
        //Post method
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(ProductVM obj, IFormFile file)
        //{
           
        //    if (ModelState.IsValid)
        //    {
        //        //_unitOfWork.Product.Add(obj.Product);
        //        _unitOfWork.Save();
        //        TempData["success"] = "CoverType created successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View(obj);
        //}

        //Get
        public IActionResult Upsert(int? id)
        {
            Company company = new();
            if (id == null || id == 0)
            {
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(company);
            }
            else
            {
                company = _unitOfWork.Company.GetFirstOrDefault(u=>u.Id==id);
                return View(company);
            }
            
        }
        //Post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj, IFormFile? file)
        {
            
            if(ModelState.IsValid)
            {
                
                if(obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["success"] = "Company created successfully";
                }
                else 
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["success"] = "Company created successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    //var cartegoryFromDb = _db.Categories.Find(id);
        //    var CoverTypeFromFirst = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
        //    //var categoryFromSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
        //    if (CoverTypeFromFirst == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(CoverTypeFromFirst);
        //}
        //Post method
        
        //public IActionResult DeletePost(int? id)
        //{
        //    var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Product.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "CoverType deleted successfully";
        //    return RedirectToAction("Index");
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data= companyList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion 
    }
}

