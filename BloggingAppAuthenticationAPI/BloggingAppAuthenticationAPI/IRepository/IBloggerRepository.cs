using BloggingAppAuthenticationAPI.Model;
using BloggingAppAuthenticationAPI.UIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloggingAppAuthenticationAPI.IRepository
{
    public interface IBloggerRepository
    {
        Task<BloggerOutGoingInfo> Authenticate(BloggerIncomingInfo bloggerInfo);
        Task<BloggerOutGoingInfo> Register(BloggerRegisteringInfo blogger);
    }
}
