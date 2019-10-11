using CodeGenerate.vsix.CodeGenerateContext;
using CodeGenerate.vsix.Common.Helper;
using EnvDTE;
using EnvDTE80;
using System;
using WinForm = System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using ProjectItem = EnvDTE.ProjectItem;
using System.Windows.Controls;

namespace CodeGenerate.vsix.Form
{
    /// <summary>
    /// MainForm.xaml 的交互逻辑
    /// </summary>
    public partial class MainForm : System.Windows.Window
    {
        public static DTE2 _dte;
        public MainForm(DTE2 dte)
        {
            _dte = dte;
            InitializeComponent();
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            if (_dte.SelectedItems.Count > 0)
            {
                SelectedItem selectedItem = _dte.SelectedItems.Item(1);
                ProjectItem selectProjectItem = selectedItem.ProjectItem;
                if (selectProjectItem != null)
                {
                    //获取当前解决方案里面的项目列表
                    var solutionProjects = _dte.Solution.Projects.OfType<Project>().ToList();
                    foreach (var item in solutionProjects) { 
                       
                    }
                    //当前项目根命名空间
                    string applicationStr = "";
                    //获取当前点击的类所在的项目
                    Project topProject = selectProjectItem.ContainingProject;
                    //当前类在当前项目中的目录结构
                    string dirPath = CommonHelper.GetSelectFileDirPath(topProject, selectProjectItem);
                    CodeClass codeClass = CommonHelper.GetClass(selectProjectItem.FileCodeModel.CodeElements);

                    //当前类命名空间
                    string namespaceStr = selectProjectItem.FileCodeModel.CodeElements.OfType<CodeNamespace>().First().FullName;
                    if (!string.IsNullOrEmpty(namespaceStr))
                    {
                        var firstIndex = namespaceStr.IndexOf(".");
                        applicationStr = namespaceStr.Substring(0, namespaceStr.IndexOf(".", firstIndex + 1));
                    }
                    Project applicationProject = solutionProjects.Find(t => t.Name == applicationStr + ".Application");
                    var applicationNewFolder = applicationProject.ProjectItems.Item(dirPath);
                    if (applicationNewFolder == null)
                    {
                        applicationNewFolder = applicationProject.ProjectItems.AddFolder(dirPath);
                    }
                    // 上下文
                    CodeContext.Instance.SelectedClass = GetSelectedClass(applicationStr, codeClass.Name, "", "", codeClass);
                    CodeContext.Instance.DirPath = dirPath;
                    CodeContext.Instance.BaseNamespaceStr = namespaceStr;
                    CodeContext.Instance.ApplicationProject = applicationProject;
                }

            }
        }

        public void Connect(int connectionId, object target)
        {
           
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CodeContext.Instance.GenerateServiceCode();
        }
        /// <summary>
        /// 获取SelectedClass
        /// </summary>
        /// <param name="applicationStr"></param>
        /// <param name="name"></param>
        /// <param name="codeClass"></param>
        /// <returns></returns>
        private SelectedClass GetSelectedClass(string applicationStr, string name, string cnName, string description,  CodeClass codeClass)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var selectEdClass = new SelectedClass(applicationStr, name);
            List<ClassProp> classProps = new List<ClassProp>();
            var codeMembers = codeClass.Members;
            foreach (CodeElement codeMember in codeMembers)
            {
                List<ClassPropAttribute> classAttributes = new List<ClassPropAttribute>();
                // 如果是属性
                if (codeMember.Kind == vsCMElement.vsCMElementProperty)
                {
                   
                    CodeProperty property = codeMember as CodeProperty;
                    ClassProp classProp = new ClassProp(property.Name);
                   
                     //获取属性类型
                    var propertyType = property.Type;
                    switch (propertyType.AsFullName)
                    {
                        case "System.Int64":
                            classProp.PropertyType = "long";
                            break;
                        case "System.Nullable<System.Int64>":
                            classProp.PropertyType = "long?";
                            break;
                        case "System.Int32":
                            classProp.PropertyType = "int";
                            break;
                        case "System.Nullable<System.Int32>":
                            classProp.PropertyType = "int?";
                            break;
                        case "System.DateTime":
                            classProp.PropertyType = "DateTime";
                            break;
                        case "System.Nullable<System.DateTime>":
                            classProp.PropertyType = "DateTime?";
                            break;
                        case "System.Guid":
                            classProp.PropertyType = "Guid";
                            break;
                        case "System.Nullable<System.Guid>":
                            classProp.PropertyType = "Guid?";
                            break;
                        case "System.String":
                            classProp.PropertyType = "string";
                            break;
                        case "System.Nullable<System.String>":
                            classProp.PropertyType = "string?";
                            break;
                         //枚举
                        default:
                            classProp.PropertyType = propertyType.AsFullName.Split('.').LastOrDefault();
                            break;
                    }
                    //获取属性特性
                    foreach (CodeAttribute codeAttribute in property.Attributes)
                    {
                        ClassPropAttribute classAttribute = new ClassPropAttribute();
                        if (codeAttribute.Name == "Required")
                        {
                            classAttribute.NameValue = "[Required]";

                            classAttribute.Name = "Required";
                            classAttribute.Value = "true";
                        }
                        else
                        {
                            classAttribute.NameValue = "[" + codeAttribute.Name + "(" + codeAttribute.Value + ")]";
                            classAttribute.Name = codeAttribute.Name;
                            classAttribute.Value = codeAttribute.Value;
                        }
                        classAttributes.Add(classAttribute);
                    }
                    classProps.Add(classProp);
                }

            }

            selectEdClass.CodeProps = classProps;

            return selectEdClass;
        }
        // 生成前端代码
        private void IsGenerateFrontChecked(object sender, RoutedEventArgs e)
        {
            this.OpenFileButton.Visibility = Visibility.Visible;
        }
        // 不生成前端代码
        private void IsGenerateFrontUnChecked(object sender, RoutedEventArgs e)
        {
            this.OpenFileButton.Visibility = Visibility.Hidden;
            CodeContext.Instance.CodeGenerateSetting.FrontPath ="";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WinForm.FolderBrowserDialog dialog = new WinForm.FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            if (dialog.ShowDialog() == WinForm.DialogResult.OK|| dialog.ShowDialog() == WinForm.DialogResult.Yes)
            {
                CodeContext.Instance.CodeGenerateSetting.FrontPath = dialog.SelectedPath;
            }

        }
    }
}
