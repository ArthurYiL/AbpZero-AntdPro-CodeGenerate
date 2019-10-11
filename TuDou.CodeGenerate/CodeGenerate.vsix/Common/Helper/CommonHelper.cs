using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerate.vsix.Common.Helper
{
    public static class CommonHelper
    {
        /// <summary>
        /// 首字母小写写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToLower(this string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToLower() + input.Substring(1);
            return str;
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToUpper() + input.Substring(1);
            return str;
        }

        /// <summary>
        /// 获取解决方案里面所有项目
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        public static List<ProjectItem> GetSolutionProjects(Solution solution)
        {
            List<ProjectItem> projectItemList = new List<ProjectItem>();
            var projects = solution.Projects.OfType<Project>();
            foreach (var project in projects)
            {
                var projectitems = GetProjects(project.ProjectItems);

                foreach (var projectItem in projectitems)
                {
                    projectItemList.Add(projectItem);
                }
            }

            return projectItemList;
        }
        /// <summary>
        /// 获取所有项目
        /// </summary>
        /// <param name="projectItems"></param>
        /// <returns></returns>
        public static IEnumerable<ProjectItem> GetProjects(ProjectItems projectItems)
        {
            foreach (ProjectItem item in projectItems)
            {
                yield return item;

                if (item.SubProject != null)
                {
                    foreach (ProjectItem childItem in GetProjects(item.SubProject.ProjectItems))
                        if (childItem.Kind == EnvDTE.Constants.vsProjectItemKindSolutionItems)
                            yield return childItem;
                }
                else
                {
                    foreach (ProjectItem childItem in GetProjects(item.ProjectItems))
                        if (childItem.Kind == EnvDTE.Constants.vsProjectItemKindSolutionItems)
                            yield return childItem;
                }
            }
        }
  
        /// <summary>
        /// 获取类
        /// </summary>
        /// <param name="codeElements"></param>
        /// <returns></returns>
        public static CodeClass GetClass(CodeElements codeElements)
        {
            var elements = codeElements.Cast<CodeElement>().ToList();
            var result = elements.FirstOrDefault(codeElement => codeElement.Kind == vsCMElement.vsCMElementClass) as CodeClass;
            if (result != null)
            {
                return result;
            }
            foreach (var codeElement in elements)
            {
                result = GetClass(codeElement.Children);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
        /// <summary>
        /// 添加文件到项目中
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        public static void AddFileToProjectItem(Project folder, string content, string fileName,string nameSpace)
        {
            try
            {
                string path = Path.GetTempPath();
                Directory.CreateDirectory(path);
                string file = Path.Combine(path, fileName);
                File.WriteAllText(file, content, Encoding.UTF8);
                try
                {
                    var folderName = nameSpace + "s";
                    var projectItem = folder.ProjectItems.Item(folderName);
                    if (projectItem == null) {
                         projectItem = folder.ProjectItems.AddFolder(folderName);
                    }
                    projectItem.ProjectItems.AddFromFileCopy(file);
                }
                finally
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 添加文件到指定目录
        /// </summary>
        /// <param name="directoryPathOrFullPath"></param>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        public static void AddFileToDirectory(string directoryPathOrFullPath, string content, string fileName = "")
        {
            try
            {
                string file = string.IsNullOrEmpty(fileName) ? directoryPathOrFullPath : Path.Combine(directoryPathOrFullPath, fileName);
                File.WriteAllText(file, content, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 获取当前所选文件去除项目目录后的文件夹结构
        /// </summary>
        /// <param name="selectProjectItem"></param>
        /// <returns></returns>
        public static string GetSelectFileDirPath(Project topProject, ProjectItem selectProjectItem)
        {
            string dirPath = "";
            if (selectProjectItem != null)
            {
                //所选文件对应的路径
                string fileNames = selectProjectItem.FileNames[0];
                string selectedFullName = fileNames.Substring(0, fileNames.LastIndexOf('\\'));

                //所选文件所在的项目
                if (topProject != null)
                {
                    //项目目录
                    string projectFullName = topProject.FullName.Substring(0, topProject.FullName.LastIndexOf('\\'));

                    //当前所选文件去除项目目录后的文件夹结构
                    dirPath = selectedFullName.Replace(projectFullName, "");
                }
            }

            return dirPath.Substring(1);
        }
    }
}
