using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloggingAppAuthenticationAPI.IRepository;
using BloggingAppAuthenticationAPI.UIModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppAuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class JWTController : ControllerBase
    {

        private readonly IBloggerRepository _bloggerRepository;

        public JWTController(IBloggerRepository bloggerRepository)
        {
            _bloggerRepository = bloggerRepository;
        }

        [HttpPost]
        [Route("Authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] BloggerIncomingInfo bloggerInfo)
        {
            BloggerOutGoingInfo token = await _bloggerRepository.Authenticate(bloggerInfo);
            if (token == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(token);
            }
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] BloggerRegisteringInfo bloggerRegisteringInfo)
        {
            BloggerOutGoingInfo token = await _bloggerRepository.Register(bloggerRegisteringInfo);
            if (token == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(token);
            }
        }
    }
}