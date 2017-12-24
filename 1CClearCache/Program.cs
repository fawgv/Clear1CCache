using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1CClearCache
{
    class Program
    {
        static void Main(string[] args)
        {

            CopyMethods copyMethods = new CopyMethods();
            copyMethods.KillRemoteProcess("1cv8*");
            copyMethods.Delete1CCache();
        }
    }
}
