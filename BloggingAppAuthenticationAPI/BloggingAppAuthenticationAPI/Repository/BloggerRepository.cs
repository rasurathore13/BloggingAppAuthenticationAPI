using BloggingAppAuthenticationAPI.IRepository;
using BloggingAppAuthenticationAPI.Model;
using BloggingAppAuthenticationAPI.UIModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BloggingAppAuthenticationAPI.Repository
{
    public class BloggerRepository : IBloggerRepository
    {
        private readonly BloggerDbContext _dbContext;
        private readonly IConfiguration _config;

        public BloggerRepository(BloggerDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        public async Task<BloggerOutGoingInfo> Authenticate(BloggerIncomingInfo bloggerInfo)
        {
            Blogger blogger = await _dbContext.Bloggers
                                              .Where(x => x.BloggerEmail == bloggerInfo.BloggerEmail)
                                              .FirstOrDefaultAsync();
            if (blogger == null)
            {
                return null;
            }
            string submittedPasswordWithSalt = bloggerInfo.BloggerPassword + blogger.BloggerSalt;
            HashAlgorithm algorithm = new SHA256Managed();
            string hashedPassword = Convert.ToBase64String(algorithm.ComputeHash(
                    Encoding.UTF8.GetBytes(submittedPasswordWithSalt)));
            if (hashedPassword == blogger.BloggerPasswordHash)
            {

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                
                Claim[] claims = new Claim[] {
                    new Claim("blogger_name", blogger.BloggerFullName.ToString()),
                    new Claim("blogger_email", blogger.BloggerEmail.ToString()),
                };
                JwtSecurityToken TokenDescriptor = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: credentials
                );

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.WriteToken(TokenDescriptor);
                BloggerOutGoingInfo bloggerOutGoingInfo = new BloggerOutGoingInfo()
                {
                    Token = token,
                };
                return bloggerOutGoingInfo;
            }
            else
            {
                return null;
            }
        }

        public async Task<BloggerOutGoingInfo> Register(BloggerRegisteringInfo bloggerRegisteringInfo)
        {
            Blogger blogger = await _dbContext.Bloggers
                                        .Where(x => x.BloggerEmail == bloggerRegisteringInfo.BloggerEmail)
                                        .FirstOrDefaultAsync();
            if (blogger != null)
            {
                BloggerIncomingInfo bloggerIncomingInfo = new BloggerIncomingInfo()
                {
                    BloggerEmail = bloggerRegisteringInfo.BloggerEmail,
                    BloggerPassword = bloggerRegisteringInfo.BloggerPassword,
                };
                BloggerOutGoingInfo bloggerOutGoingInfo = await Authenticate(bloggerIncomingInfo);
                return bloggerOutGoingInfo;
            }

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByteText = new byte[96];
            rng.GetBytes(saltByteText);

            Blogger newBlogger = new Blogger()
            {
                BloggerEmail = bloggerRegisteringInfo.BloggerEmail,
                BloggerFullName = bloggerRegisteringInfo.BloggerFullName,
                BloggerDOB = bloggerRegisteringInfo.BloggerDOB
            };

            newBlogger.BloggerSalt = Convert.ToBase64String(saltByteText);
            string passwordWithSalt = bloggerRegisteringInfo.BloggerPassword + newBlogger.BloggerSalt;

            HashAlgorithm algorithm = new SHA256Managed();
            newBlogger.BloggerPasswordHash = Convert.ToBase64String(algorithm.ComputeHash(
                Encoding.UTF8.GetBytes(passwordWithSalt)));

            await _dbContext.Bloggers.AddAsync(newBlogger);
            await _dbContext.SaveChangesAsync();

            return await Register(bloggerRegisteringInfo);
        }
    }
}
