using CodeGenerate.vsix.Common;

namespace CodeGenerate.vsix.CodeGenerateContext
{
    public class CodeGenerateSetting
    {
        /// <summary>
        /// 前端文件路径
        /// </summary>
        public string FrontPath { get; set; }
        /// <summary>
        /// 生成代码类型
        /// </summary>
        public GenerateType GenerateType { get; set; }
    }
}
