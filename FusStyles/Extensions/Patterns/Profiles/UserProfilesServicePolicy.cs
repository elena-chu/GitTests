using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Patterns.Profiles
{
    public enum UserProfilesServicePolicy
    {
        /// <summary>
        /// Get only factory profiles
        /// </summary>
        FactoryOnly,
        /// <summary>
        /// Get only profiles of the user currently loggen in. NOTE, the user loggen in may be a factory user. In this case the result is identical to <see cref="FactoryOnly"/>
        /// </summary>
        UserOnly,
        /// <summary>
        /// Get profiles both factory and from the user currently logged in. When there is a profile with same name take factory.
        /// </summary>
        BothFactoryOverrides,
        /// <summary>
        /// Get profiles both factory and from the user currently logged in. When there is a profile with same name take user.
        /// </summary>
        BothUserOverrides,
    }
}
