using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vkontakte
{
    public class MethodLibraryBase
    {
        protected IVkAdapter Adapter;

        protected MethodLibraryBase() {}
        protected MethodLibraryBase(IVkAdapter vkAdapter)
        {
            this.Adapter = vkAdapter;
        }
    }
}
