using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace file.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login(string action,FormCollection form)
        {
            //session赋值，用以判断用户状态，是否登录，以及其他地方获取UserName或UserId
            if (true)//判断用户名和密码是否匹配
            {

                //Session["Id"] = form.Id;
                //Session["UserName"] = form.Name;
            }
            return View();
        }
    }
}