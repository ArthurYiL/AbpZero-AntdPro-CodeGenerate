using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerate.vsix.Models
{
    public class ServiceFileModel
    {
        public string Namespace { get; set; }

        public string Name { get; set; }

        public string DirName { get; set; }

        public string CnName { get; set; }
    }
}
