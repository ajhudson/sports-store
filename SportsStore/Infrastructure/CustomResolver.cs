using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace SportsStore.Infrastructure
{
    public class CustomResolver : IDependencyResolver, IDependencyScope
    {
        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
           // do nothing - not required
        }

        public object GetService(Type serviceType)
        {
            return serviceType == typeof(IRepository) ? new ProductRepository() : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Enumerable.Empty<object>();
        }
    }
}