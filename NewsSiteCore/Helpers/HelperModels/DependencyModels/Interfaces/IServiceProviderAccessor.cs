using System;
using System.Collections.Generic;
using System.Text;

namespace Models.HelperModels.DependencyModels.Interfaces
{
    public interface IServiceProviderAccessor
    {
        IServiceProvider ServiceProvider { get; set; }
    }
}
