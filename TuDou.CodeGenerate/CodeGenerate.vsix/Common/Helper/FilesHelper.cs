using CodeGenerate.vsix.Models;
using EnvDTE;
using System.IO;
using System.Text;
using Engine = RazorEngine.Engine;
using RazorEngine.Templating;
namespace CodeGenerate.vsix.Common.Helper
{
    public static class FilesHelper
    {
        /// <summary>
        /// 创建Service类
        /// </summary>
        /// <param name="applicationStr">根命名空间</param>
        /// <param name="name">类名</param>
        /// <param name="dtoFolder">父文件夹</param>
        /// <param name="dirName">类所在文件夹目录</param>
        private static void CreateServiceFile(string applicationStr, string name, string cnName, Project dtoFolder, string dirName, CodeClass codeClass)
        {
            var model = new ServiceFileModel() { Namespace = applicationStr, Name = name, CnName = cnName, DirName = dirName.Replace("\\", ".") };
            string path = @"D:\个人程序文件\个人项目\TuDou.CodeGenerate\CodeGenerate.vsix\Templates\IServiceTemplate.cshtml";
            var template = File.ReadAllText(path);
            string content_IService = Engine.Razor.RunCompile(template, "IServiceTemplate", null, model);
            string fileName_IService = $"I{name}AppService.cs";
            CommonHelper.AddFileToProjectItem(dtoFolder, content_IService, fileName_IService,name);

            string content_Service = Engine.Razor.RunCompile("ServiceTemplate", typeof(ServiceFileModel), model);
            string fileName_Service = $"{name}AppService.cs";
            CommonHelper.AddFileToProjectItem(dtoFolder, content_Service, fileName_Service, name);
        }
        public static string Write(string directory, string fileName, string content)
        {
            var path = Path.Combine(directory, fileName + ".cs");
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                byte[] byteFile = Encoding.UTF8.GetBytes(content);
                fs.Write(byteFile, 0, byteFile.Length);
            }
            return path;
        }
    }
}
