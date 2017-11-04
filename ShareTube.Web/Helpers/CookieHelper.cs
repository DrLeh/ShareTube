using Microsoft.AspNetCore.Http;
using ShareTube.Core;
using ShareTube.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareTube.Web.Helpers
{
    //public class CookieHelper : ICookieHelper
    //{
    //    public Guid GetOrAddUserID()
    //    {
    //        var existingCookie = HttpContext.Request.Cookies[Constants.UserIDCookieName];
    //        if (existingCookie != null)
    //            return Guid.Parse(existingCookie.Value);
    //        else
    //        {
    //            var id = Guid.NewGuid();
    //            var cookie = new HttpCookie(Constants.UserIDCookieName, id.ToString());
    //            HttpContext.Current.Response.Cookies.Add(cookie);
    //            return id;
    //        }
    //    }
    //}
}
