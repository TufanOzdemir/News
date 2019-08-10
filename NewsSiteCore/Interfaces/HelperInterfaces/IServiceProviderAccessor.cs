using System;

namespace Interfaces.HelperInterfaces
{
    public interface IServiceProviderAccessor
    {
        IServiceProvider ServiceProvider { get; set; }
    }
}
