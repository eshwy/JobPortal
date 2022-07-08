using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JopPortal.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace JopPortal.Repository
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
		private readonly IConfiguration iconfiguration;
		private readonly JobPortal2Context _context;
		public JWTManagerRepository(IConfiguration iconfiguration, JobPortal2Context context)
		{
			this.iconfiguration = iconfiguration;
			_context = context;
		}
		public string Authenticate(LoginTbl users)
		{
			var data = _context.LoginTbls.FirstOrDefault(x => x.UserName == users.UserName && x.PassWord == users.PassWord);
			
			if (data is null)
            {
                return null;
            }

            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
			  {
			 new Claim(ClaimTypes.Name, data.UserName),
			 new Claim(ClaimTypes.NameIdentifier, data.UserId.ToString()),
			
			 
			  }),
				Expires = DateTime.UtcNow.AddMinutes(60),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return new string(new JwtSecurityTokenHandler().WriteToken(token));

		}
	}
}
