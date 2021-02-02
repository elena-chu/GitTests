using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Patterns.Profiles
{
    public class Profile
    {
        public virtual ProfileName Name { get; set; }
        public virtual string Description { get; set; }
        public virtual int UserId { get; set; }

        public virtual object DataObj { get; set; }
    }

    public class Profile<T> : Profile
    {
        public virtual T Data { get; set; }

        public override object DataObj
        {
            get { return Data; }
            set { Data = (T)value; }
        }
    }
}
