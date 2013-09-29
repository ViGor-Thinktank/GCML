using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using GcmlClientWebMVC.Models;

namespace GcmlClientWebMVC
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // Damit sich Benutzer dieser Website mithilfe ihrer Konten von anderen Websites (z. B. Microsoft, Facebook und Twitter) anmelden können,
            // müssen Sie diese Website aktualisieren. Weitere Informationen finden Sie unter "http://go.microsoft.com/fwlink/?LinkID=252166".

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
