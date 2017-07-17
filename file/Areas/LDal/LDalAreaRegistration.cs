using System.Web.Mvc;

namespace file.Areas.LDal
{
    public class LDalAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LDal";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LDal_default",
                "LDal/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}