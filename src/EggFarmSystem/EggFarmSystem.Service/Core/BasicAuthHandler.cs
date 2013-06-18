using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Principal;
using EggFarmSystem.Models;
using EggFarmSystem.Services;


namespace EggFarmSystem.Service.Core
{
    public class BasicAuthHandler : DelegatingHandler
    {
        private const string Scheme = "BASIC";

        private IAccountService accountService;

        public BasicAuthHandler() : this(null)
        {
        }

        public BasicAuthHandler(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (accountService == null)
                accountService = request.GetDependencyScope().GetService(typeof (IAccountService)) as IAccountService;

            var headers = request.Headers;

            if(headers.Authorization == null || headers.Authorization.Scheme != Scheme)
                return base.SendAsync(request, cancellationToken);
            
            var values = Encoding.ASCII.GetString(Convert.FromBase64String(headers.Authorization.Parameter)).Split(':');
            string userName = values[0].Trim();
            string password = values[1].Trim();
            //var account = accountService.Get(userName, password);
            
            //if (account == null)
            //    return base.SendAsync(request, cancellationToken);

            //var principal = new GenericPrincipal(new GenericIdentity(userName),
            //                                        new[] {((RoleType) account.RoleType).ToString()});


            var principal = new GenericPrincipal(new GenericIdentity(userName),
                                                    new[] {"Admin"});
            
            Thread.CurrentPrincipal = principal;    
            request.Properties.Add("user", principal);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
