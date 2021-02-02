using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Patterns.Profiles
{
    public class ProfileException : Exception
    {
        public ProfileName ProfileName { get; }

        public ProfileException()
        {
        }

        public ProfileException(string message) : base(message)
        {
        }

        public ProfileException(string message, Exception inner) : base(message, inner)
        {
        }

        public ProfileException(ProfileName pn, string message) : this(message)
        {
            ProfileName = pn;
        }

        public ProfileException(ProfileName pn, string message, Exception inner) : this(message, inner)
        {
            ProfileName = pn;
        }
    }
}
