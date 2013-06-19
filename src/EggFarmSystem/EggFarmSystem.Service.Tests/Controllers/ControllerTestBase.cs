using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using EggFarmSystem.Models;
using Xunit;

namespace EggFarmSystem.Service.Tests.Controllers
{
    public class ControllerTestBase
    {
        protected void AssertPropertyError(HttpResponseMessage message, string propertyName)
        {
            var errorList = (message.Content as ObjectContent<IList<ErrorInfo>>).Value as IList<ErrorInfo>;
            Assert.True(errorList.Any(c => c.PropertyName == propertyName));
        }
    }
}
