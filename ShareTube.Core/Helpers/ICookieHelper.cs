using System;
using System.Collections.Generic;
using System.Text;

namespace ShareTube.Core.Helpers
{
    public interface ICookieHelper
    {
        Guid GetOrAddUserID();
    }
}
