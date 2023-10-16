using Microsoft.IdentityModel.Tokens;
using SharedObjects.TockenHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Support
{
    public class TokensContainer
    {

        private static ITokenHandler TockenHandler { get; }


        public static  String GenerateTocken(String Username)
        {
            return  TockenHandler.GenerateToken(Username);
          
            
        }

        public static bool AuthenticateClient(String token)
        {
            return TockenHandler.ValidateToken(token); 
         
        }


        static  TokensContainer()
        {
            TockenHandler  = new TokenHandlerClass("IamASecret");
        }
    }
}
