using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerate.vsix.CodeGenerateContext
{
   public class ClassPropAttribute
    {
        /// <summary>
        /// 特性名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 特性值
        /// </summary>
        public string Value { get; set; }

        public string NameValue { get; set; }
        /// <summary>
        /// 命名空间
        /// </summary>
        public string NameSpace { get; set; }
    }
}
