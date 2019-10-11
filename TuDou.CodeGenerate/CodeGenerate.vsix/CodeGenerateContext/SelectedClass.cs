using System.Collections.Generic;

namespace CodeGenerate.vsix.CodeGenerateContext
{
    /// <summary>
    /// 选中的类
    /// </summary>
    public class SelectedClass
    {
        public SelectedClass(string namePlace,string className)
        {
            NamePlace = namePlace;
            ClassName = className;
        }
        /// <summary>
        /// 命名空间
        /// </summary>
        public string NamePlace { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 中文名称
        /// </summary>
        public string CNName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DirName { get; set; }
        /// <summary>
        /// 类属性
        /// </summary>
        public List<ClassProp> CodeProps { get; set; }
        
        /// <summary>
        /// 类继承类
        /// </summary>
        public List<ClassBaseClass> ClassBaseClasss { get; set; }
    }
}
