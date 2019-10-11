using CodeGenerate.vsix.Templates;
using EnvDTE;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.IO;
using Engine = RazorEngine.Engine;
namespace CodeGenerate.vsix.CodeGenerateContext
{
    /// <summary>
    /// 生成代码上下文
    /// </summary>
    public class CodeContext
    {
        static readonly string TemplateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
        static CodeContext()
        {
            Instance = new CodeContext();
            Instance.CodeGenerateSetting = new CodeGenerateSetting();
            InitRazorEngine();
        }
        private CodeContext()
        {

        }
        /// <summary>
        /// .Application层项目
        /// </summary>
        public Project ApplicationProject { get; set; }
        public static CodeContext Instance { get; private set; } = null;

        public string BaseNamespaceStr { get; set; }
        /// <summary>
        /// 所在项目目录结构
        /// </summary>
        public string DirPath { get; set; }
        /// <summary>
        /// 选中的类
        /// </summary>
        public SelectedClass SelectedClass { get; set; }
        /// <summary>
        /// 设置
        /// </summary>
        public CodeGenerateSetting CodeGenerateSetting { get; set; }

        /// <summary>
        /// 初始化Razor
        /// </summary>
        private static void InitRazorEngine()
        {

            var config = new TemplateServiceConfiguration
            {
                TemplateManager = new EmbeddedResourceTemplateManager(typeof(Template))
            };
            Engine.Razor = RazorEngineService.Create(config);
        }
        public void GenerateServiceCode()
        {
            GenerateHelper.CreateServiceFile(BaseNamespaceStr, SelectedClass.ClassName, SelectedClass.CNName, ApplicationProject, DirPath);
        }
    }
}
