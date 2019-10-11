using CodeGenerate.vsix.Common.Helper;
using CodeGenerate.vsix.Models;
using EnvDTE;
using EnvDTE80;
using RazorEngine.Templating;
using Engine = RazorEngine.Engine;
namespace CodeGenerate.vsix.CodeGenerateContext
{
    /// <summary>
    /// 生成代码
    /// </summary>
    public static class GenerateHelper
    {
       
        /// <summary>
        /// 创建Service类
        /// </summary>
        /// <param name="applicationStr">根命名空间</param>
        /// <param name="name">类名</param>
        /// <param name="dtoFolder">父文件夹</param>
        /// <param name="dirName">类所在文件夹目录</param>
        public static void CreateServiceFile(string applicationStr, string name, string cnName, Project dtoFolder, string dirName)
        {
            var model = new ServiceFileModel() { Namespace = applicationStr, Name = name, CnName = cnName, DirName = dirName.Replace("\\", ".") };

            string content_IService = Engine.Razor.RunCompile("IServiceTemplate", typeof(ServiceFileModel), model);
            string fileName_IService = $"I{name}AppService.cs";
            CommonHelper.AddFileToProjectItem(dtoFolder, content_IService, fileName_IService, name);

            //string content_Service = Engine.Razor.RunCompile("ServiceTemplate", typeof(ServiceFileModel), model);
            //string fileName_Service = $"{name}AppService.cs";
            //CommonHelper.AddFileToProjectItem(dtoFolder, content_Service, fileName_Service);
        }

    }
}
