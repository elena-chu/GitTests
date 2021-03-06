﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ws.Extensions.Patterns.Profiles
{
    public interface IProfilesMgrFactory
    {
        Task<IProfilesMgr> CreateObjAsync(ProfileServices profileServices, UserProfilesServicePolicy policy, CancellationToken ct = default(CancellationToken));
    }
}
