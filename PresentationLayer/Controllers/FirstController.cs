using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class FirstController : Controller
    {
        // 🚩🚩 Action
        //===========================
        //      ✅ must end with ....{Controller}
        //      ✅ must be public
        //      ✅ cannot be overloaded
        //      ✅ can return string , json , view , files

        //      ✅ Result Data Types

        //      ✅ String       ===>   ContentResult
        //      ✅ View         ===>   ViewResult
        //      ✅ Javascript   ===>   JavascriptResult
        //      ✅ JSON         ===>   JsonResult
        //      ✅ Files        ===>   FileResult
        //public IActionResult Index()
        //{
        //    return View();
        //}

        // 🚀❌ No Overloads 
        //public IActionResult Index(int x)
        //{
        //    return View();
        //}


        // Dynamic Routing   ===>      /Contoller_Name/Method_Name

        //   /First/Welcome
        //public string Welcome()
        //{
        //    return "Welcome in my first page";
        //}
        //public string Welcome(int id)         ====>  ❌❌
        //{
        //    return "Welcome in my first page";
        //}
        public ContentResult Welcome()
        //public IActionResult Welcome()
        {
            var result = new ContentResult();
            result.Content = "Welcome in my first page";
            return result;
        }
        public JsonResult getJson()
        //public IActionResult getJson()
        {
            //var result = new JsonResult(new { Id = 1, Name = "Mazen" });
            //return result;

            return Json(new { Id = 1, Name = "Mazen" });
        }
        // All Result classes inherit form ActionResult and implements IActionResult

        //  InvalidOperationException: The view 'MyView' was not found.The following locations were searched:
        //   /Views/First/MyView.cshtml
        //   /Views/Shared/MyView.cshtml

        public IActionResult getMix()
        {
            if(DateTime.Now.Day == 6)
            {
                //var result = new ContentResult();
                //result.Content = "Page Closed";
                //return result;

                return Content("Page Closed");
            }
            else
            {
                //var result = new ViewResult();
                //result.ViewName = "MyView";
                //return result;

                return View("MyView");
            }
        }
    }
}
