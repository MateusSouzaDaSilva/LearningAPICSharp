﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LearningWebAPI.Domain.Model.EmployeeAggregate;
using Microsoft.IdentityModel.Tokens;

namespace LearningWebAPI.Application.Services
{
    public class TokenService
    {

        // Gera o token JWT
        public static object GenerateToken(Employee employee)
        {
            var key = Encoding.ASCII.GetBytes(Key.Secret);
            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("employeeId", employee.id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            var tokenString = tokenHandler.WriteToken(token);

            return new
            {
                token = tokenString
            };
        }
    }
}
