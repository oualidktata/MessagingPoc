using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace DispatcherService
{
    class Program
    {
        static int Main(string[] args)
        {
            return (int) HostFactory.Run(x=>x.Service<NestorService.NestorService>());
        }
    }
}
