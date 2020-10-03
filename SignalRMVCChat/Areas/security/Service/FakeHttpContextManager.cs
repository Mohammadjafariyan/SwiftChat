using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using SignalRMVCChat.Areas.security.Controllers;

namespace SignalRMVCChat.Areas.security.Service
{
    public class FakeHttpContextManager 
    {
        private static HttpContextBase m_context;
        public static HttpContextBase Current
        {
            get
            {
                if (m_context != null)
                    return m_context;

                if (HttpContext.Current == null)
                    throw new InvalidOperationException("HttpContext not available");

                return new HttpContextWrapper(HttpContext.Current);
            }
        }

        public static void SetCurrentContext(HttpContextBase context)
        {
            m_context = context;
        }

        public static void init(Controller acc)
        {
            FakeHttpContextManager.SetCurrentContext(GetMockedHttpContext());
            
            Mock<ControllerContext> controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.Setup(
                x => x.HttpContext
            ).Returns(FakeHttpContextManager.Current);
            
            acc.ControllerContext = controllerContextMock.Object;
        }
        
        private static HttpContextBase GetMockedHttpContext()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();
            var urlHelper = new Mock<UrlHelper>();

          //  var routes = new RouteCollection();
            //RouteConfig.RegisterRoutes(routes);

            var requestContext = new Mock<RequestContext>();
            requestContext.Setup(x => x.HttpContext).Returns(context.Object);
            
            
            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            
            
           var dic=new Mock<Dictionary<string,object>>();
            context.Setup(ctx => ctx.Items).Returns(dic.Object);


            context.Setup(ctx => ctx.Session).Returns(new MockHttpSession());
            
            context.Setup(ctx => ctx.Server).Returns(server.Object);
            context.Setup(ctx => ctx.User).Returns(user.Object);
            user.Setup(ctx => ctx.Identity).Returns(identity.Object);
            identity.Setup(id => id.IsAuthenticated).Returns(true);
            identity.Setup(id => id.Name).Returns("test");
            request.Setup(req => req.Url).Returns(new Uri("http://www.google.com"));
            request.Setup(req => req.RequestContext).Returns(requestContext.Object);
            requestContext.Setup(x => x.RouteData).Returns(new RouteData());
            request.SetupGet(req => req.Headers).Returns(new NameValueCollection());

            return context.Object;
        }
    }
    
    public class MockHttpSession : HttpSessionStateBase
    {
        Dictionary<string, object> _sessionDictionary = new Dictionary<string, object>();
        public override object this[string name]
        {
            get
            {
                return _sessionDictionary.ContainsKey(name) ? _sessionDictionary[name] : null;
            }
            set
            {
                _sessionDictionary[name] = value;
            }
        }

        public override void Abandon()
        {
            var keys = new List<string>();

            foreach (var kvp in _sessionDictionary)
            {
                keys.Add(kvp.Key);
            }

            foreach (var key in keys)
            {
                _sessionDictionary.Remove(key);
            }
        }

        public override void Clear()
        {
            var keys = new List<string>();

            foreach (var kvp in _sessionDictionary)
            {
                keys.Add(kvp.Key);
            }

            foreach(var key in keys)
            {
                _sessionDictionary.Remove(key);
            }
        }
    }
}