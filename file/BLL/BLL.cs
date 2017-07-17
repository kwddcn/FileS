using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using file.Interface;
using file.Models;


namespace file.BLL
{
    public class BLL : LoginInterface
    {
        IDALInterface IDALInterface = new DBBLL();
        public void Login(userModel model)
        {
            //判断用户名和密码是否匹配
            if (IDALInterface.SearchOneRowOneLine<userModel>(model.Id.ToString(), "Name=" + model.Name + " and " + "Password=" + model.Password)!=null)
            {
                //session赋值，用以判断用户状态，是否登录，以及其他地方获取UserName或UserId
                System.Web.HttpContext.Current.Session["Id"] = model.Id;
                System.Web.HttpContext.Current.Session["UserName"] = model.Name;
            }
        }
    }
}