using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareTube.Infrastructure
{
    public class CookieHelper : ICookieHelper
    {
        public Guid GetOrAddUserID()
        {
            var existingCookie = HttpContext.Current.Request.Cookies[Constants.UserIDCookieName];
            if (existingCookie != null)
                return Guid.Parse(existingCookie.Value);
            else
            {
                var id = Guid.NewGuid();
                var cookie = new HttpCookie(Constants.UserIDCookieName, id.ToString());
                HttpContext.Current.Response.Cookies.Add(cookie);
                return id;
            }
        }
    }
    public interface ICookieHelper
    {
         Guid GetOrAddUserID();
    }
}