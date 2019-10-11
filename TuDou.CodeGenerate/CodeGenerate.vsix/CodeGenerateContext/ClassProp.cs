using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerate.vsix.CodeGenerateContext
{
    /// <summary>
    /// 类属性
    /// </summary>
    public class ClassProp
    {
        public ClassProp(string fullname,string cnname="")
        {
            FullName = fullname;
            CNName = cnname;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 中文名称
        /// </summary>
        public string CNName { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public string PropertyType { get; set; }
        /// <summary>
        /// 属性特性
        /// </summary>
        public List<ClassPropAttribute> ClassAttributes { get; set; }
    }
}
