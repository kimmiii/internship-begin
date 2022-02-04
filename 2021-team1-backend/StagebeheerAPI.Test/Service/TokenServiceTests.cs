
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace StagebeheerAPI.Tests.Service
{
    public class TokenServiceTests
    {

        private Mock<IConfiguration> configurationMock;
        private string issuer = "pxl.com";
        private string secret = "123456789123456789123456789123456789";

        public TokenServiceTests()
        {

            configurationMock = new Mock<IConfiguration>();
        }

        //[Test]
        //public void GenerateJSONWebToken_ReturnToken()
        //{
        //    //Arrange
        //    TokenService tokenService = new TokenService(configurationMock.Object);

        //    //Act,Assert
        //    string token = tokenService.GenerateJSONWebToken(issuer, secret);
        //    int tokenParts = token.Split('.').Length;
        //    Assert.AreEqual(3,tokenParts);

        //}


    }
}
