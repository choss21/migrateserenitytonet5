using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigrateSerenityCore
{
    public partial class FPassv3To5 : Form
    {
        public FPassv3To5()
        {
            InitializeComponent();
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            var opfd = new FolderBrowserDialog();
            if (opfd.ShowDialog() != DialogResult.OK) return;
            txtDirectorioModulos.Text = opfd.SelectedPath;
        }

        private void btnMigrar_Click(object sender, EventArgs e)
        {
            var dirProject = new DirectoryInfo(txtDirectorioModulos.Text);
            if (!dirProject.Exists) return;
            Properties.Settings.Default.LastDirectorySelected = dirProject.FullName;
            Properties.Settings.Default.Save();

            //if (MessageBox.Show("Favor de realizar respaldo antes de continuar, ya que este proceso sobreescribira los archivos existentes,¿Confirma que desea continuar?",
            //    "Confirma que desea continuar con el Proceso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No) return;



            if (chkIsNetFramework.Checked)
                TransformNetFrameworkToNetcore3_1(dirProject.FullName);
            var filesCS = Directory.EnumerateFiles(dirProject.FullName, "*.cs", SearchOption.AllDirectories).ToList();
            var filescsHtml = Directory.EnumerateFiles(dirProject.FullName, "*.cshtml", SearchOption.AllDirectories);
            filesCS.AddRange(filescsHtml);

            var tupsReplace = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>(@"([^a-zA-Z]+)Row([^a-zA-Z]+)",
                                            @"$1IRow$2"),//Replace Row to IRow Verificar todo esto que funcione correctamente
                new Tuple<string, string>(@"class\s*([A-Za-z]*)Row\s*:\s*I?Row(\s*[^<]{1})",
                                            @"class $1Row : Row<$1Row.RowFields>$2"),
                new Tuple<string, string>(@"[\r]?[\n]?[ \t]*public\s+static\s+readonly\s+RowFields\s+Fields\s*=\s*new\s+RowFields\(\)\.Init\(\);\r?\n",
                                            @""),
                new Tuple<string, string>(@"([ \t]*public[ \t]* )([A-Za-z]*)Row\(\)([\s\r\n]*:[\s]*base\()Fields\)(\s*\{\s*\})([\r]?[\n]?)",
                                            @"$1$2Row()$4$5$5$1$2Row(RowFields fields)$3fields)$4$5"),
                new Tuple<string, string>(@"(\[[^\{\}]*)(\][\r\n\s]*public [A-Za-z0-9]+\?? )([A-Za-z]+)(\s*\r?\n+[\s\S\n]+)(\r?\n\s*IIdField\s*IIdRow.IdField[\s\r\n]*(\{[\r\n\s]*get[r\n\s]*\{[\r\n\s]*return|=>\s*) Fields.)(\3)([\s]*;([\r\n\s]*\}[\r\n\s]*\}|))\r?\n?\r?\n?",
                                            @"$1, IdProperty$2$3$4"),
                new Tuple<string, string>(@"(\[[^\{\}]*)(\][\r\n\s]*public [A-Za-z0-9]+\?? )([A-Za-z]+)(\s*\r?\n+[\s\S\n]+)(\r?\n\s*StringField\s*INameRow.NameField[\s\r\n]*(\{[\r\n\s]*get[r\n\s]*\{[\r\n\s]*return|=>\s*) Fields.)(\3)([\s]*;([\r\n\s]*\}[\r\n\s]*\}|))\r?\n?",
                                            @"$1, NameProperty$2$3$4"),
                new Tuple<string, string>(@"(\s?)IIdField(\s+)",
                                            @"$1Field$2"),
                new Tuple<string, string>(@"(^[\t ]*)(get|set)[ \t]*(\=\>|\{)[\t ]*(return|)[\t ]*F(ields\.[^\^;}]*)\;[\t ]*\}?[\t ]*\r?$",
                                            @"$1$2 => f$5;"),//Using fields instead of Fields in Row Properties
                new Tuple<string, string>(@"([\t ]*)Check.NotNull\(([A-Za-z0-9\.]+),\s*([^\n\r]*)\);([\r]?\n)",
                                            @"$1if ($2 is null)$4$1    throw new ArgumentNullException($3);$4"),
                new Tuple<string, string>(@"([\t ]*)Check.NotNullOrEmpty\(([A-Za-z0-9\.]+),\s*([^\n\r]*)\);([\r]?\n)",
                                            @"$1if (string.IsNullOrEmpty($2))$4$1    throw new ArgumentNullException($3);$4"),
                new Tuple<string, string>(@"([\t ]*)Check.NotNullOrWhiteSpace\(([A-Za-z0-9\.]+),\s*([^\n\r]*)\);([\r]?\n)",
                                            @"$1if (string.IsNullOrWhiteSpace($2))$4$1    throw new ArgumentNullException($3);$4"),
                new Tuple<string, string>(@"([\t ]*)request.CheckNotNull\(\);([\r]?\n)",
                                            @"$1if (request is null)$2$1    throw new ArgumentNullException(nameof(request));$2"),
                new Tuple<string, string>(@"[\t ]*\[[\t ]*(SettingScope\(.*\)\,|)[\t ]*SettingKey\(""(.*)""\)\][\t ]*\r?\n[\s\r\n]*public class[\t ]*([A-Za-z0-9]*)[\t ]*(\r?\n)[\r\n]*([\t ]*)\{",
                                            @"$5// services.Configure<$3>(Configuration.GetSection($3.SectionKey));$4$5public class $3$4$5{$4$5    public const string SectionKey = ""$2"";$4"),
                new Tuple<string, string>(@"using Serenity\.Configuration\;[\t ]*\r?\n",
                                            @""),
                new Tuple<string, string>(@"using Serenity\.Extensibility\;(\t ]*\r?\n)",
                                            @"using Serenity.ComponentModel$1"),
                new Tuple<string, string>(@"([\t ]*)public[\t ]+class[\t ]+([A-Za-z0-9_]*)Repository[\t ]*(\r?\n|)[\t ]*\{",
                                            @"$1public class $2Repository : BaseRepository$3$1{$3$1$1public $2Repository(IRequestContext context)$3$1$1$1 : base(context)$3$1$1{$3$1$1}$3"),
                new Tuple<string, string>(@"([\t ]*)(private|public)[\t ]+class[\t ]+My(Save|Retrieve|List|Delete|Undelete)Handler[\t ]*\:[\t ]*(\3)RequestHandler(\<[A-Za-z\.\, \t]+\>)[\t ]*\{?[\t ]*(\r?\n)[\t\r\s]*\{",
                                            @"$1$2 class My$3Handler : $3RequestHandler$5$6$1{$6$1    public My$3Handler(IRequestContext context)$6$1$1 : base(context)$6$1    {$6$1    }$6"),
                new Tuple<string, string>(@"([\t ]*)(private|public)[\t ]+class[\t ]+My(Save|Retrieve|List|Delete|Undelete)Handler[\t ]*\:[\t ]*(\3)RequestHandler(\<[A-Za-z\.\, \t]+\>)[\t ]*\{?[\t ]*\}[\t ]*(\r?\n)([\t\r\s]*\n)?",
                                            @"$1$2 class My$3Handler : $3RequestHandler$5$6$1{$6$1    public My$3Handler(IRequestContext context)$6$1$1 : base(context)$6$1    {$6$1    }$6$1}$6$6"),
                new Tuple<string, string>(@"new[\t ]*My(Save|Retrieve|List|Delete|Undelete)Handler[\t ]*\([\t ]*\)",
                                            @"new My$1Handler(Context)"),
                new Tuple<string, string>(@"new[\t ]*([A-Za-z0-9_]*)Repository[\t ]*\([\t ]*\)",
                                            @"new $1Repository(Context)"),
                new Tuple<string, string>(@"class\s*([A-Za-z0-9_]*)Row\s*:\s*([A-Za-z0-9_.]*)LoggingRow\s*[^<]",
                                            @"class $1Row : $2LoggingRow<$1Row.RowFields>,"),
                new Tuple<string, string>(@"class\s*RowFields\s*:\s*([a-zA-Z0-9_.]*)LoggingRow\.LoggingRowFields",
                                            @"class RowFields : $1LoggingRowFields"),
                new Tuple<string, string>(@"BatchGenerationUpdater\.OnCommit\((uow|this\.UnitOfWork|UnitOfWork),\s*(.*).GenerationKey\)",
                                            @"Cache.InvalidateOnCommit($1, $2)"),
                new Tuple<string, string>(@"Authorization\.HasPermission\(([A-Za-z\.""]*)\)",
                                            @"Permissions.HasPermission($1)"),
                new Tuple<string, string>(@"Authorization\.ValidatePermission\(([^\r\n]+)\);",
                                            @"Permissions.ValidatePermission($1, Localizer);"),
                new Tuple<string, string>(@"Serenity\.Permissions",
                                            @"Permissions.ValidatePermission($1, Localizer);"),
                new Tuple<string, string>(@"TwoLevelCache\.ExpireGroupItems",
                                            @"Cache.ExpireGroupItems"),
                new Tuple<string, string>(@"TwoLevelCache\.Get",
                                            @"Cache.Get"),
                new Tuple<string, string>(@"TwoLevelCache\.Remove",
                                            @"Cache.Remove"),
                new Tuple<string, string>(@"Texts\.([A-Za-z_.]*)([\s,;\<\r\n)])",
                                            @"Texts.$1.ToString(Localizer)$2"),
                new Tuple<string, string>(@"Texts\.([A-Za-z_.]*)\.ToString\(\)",
                                            @"Texts.$1.ToString(Localizer)"),
                new Tuple<string, string>(@"(Serenity\.)?LocalText\.(Get|TryGet)\(([""A-Za-z_\.]+)\)",
                                            @"Localizer.$2($3)"),
                new Tuple<string, string>("UserRepository.isPublicDemo","UserRepository.IsPublicDemo"),
                new Tuple<string,string>(@"#if +COREFX\r?\n([\t A-Za-z0-9\.\;\r\n\/\*\[\]\(\)\""\,\{\}\=]*)(#endif|#else\r?\n[\t A-Za-z0-9\.\;\r\n\/\*\[\]\(\)\""\,\{\}\=]*#endif)",
                                            @"$1"),//ADVERTENCIA esta linea elimina todo lo que esta en el #else de #if COREFX
                new Tuple<string, string>(@"#if +!COREFX\r?\n([\t A-Za-z0-9\.\;\r\n\/\*\[\]\(\)\""\,\{\}\=]*)(#endif|#else\r?\n([\t A-Za-z0-9\.\;\r\n\/\*\[\]\(\)\""\,\{\}]*)#endif)",
                                            @"$3")//ADVERTENCIA esta linea elimina todo lo que esta en el #if !COREFX
            };
            //delete Modules/Common/Helpers/DetailListSaveHandler.cs
            //Take latest version of PermissionService.cs from StartSharp / Serene repository
            var obj = new Object();
            Parallel.ForEach(filesCS, file =>
            {
                if (file.Contains("\\Migrations\\"))
                {
                    return;
                }
                if (file.EndsWith("Modules/Common/Helpers/DetailListSaveHandler.cs"))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                    return;
                }
                var sText = File.ReadAllText(file);
                var sModificaciones = sText;
                foreach (var tp in tupsReplace)
                {
                    sModificaciones = Regex.Replace(sModificaciones, tp.Item1, tp.Item2);
                }
                try
                {
                    File.Delete(file);
                }
                catch (Exception wz)
                {
                    wz.ToString();
                }
                if (file.EndsWith(".cshtml"))
                {
                    if (!sModificaciones.Contains("@inject Serenity.ITextLocalizer Localizer"))
                    {
                        sModificaciones = "@inject Serenity.ITextLocalizer Localizer" + Environment.NewLine + sModificaciones;
                    }
                }

                File.WriteAllText(file, sModificaciones, Encoding.UTF8);
                this.Invoke(new Action(() =>
                {
                    try
                    {
                        lock (obj)
                        {
                            txtStatus.Text = file + Environment.NewLine + txtStatus.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }));
            });
            MessageBox.Show("Conversion finalizada", "Accion terminada", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FPassv3To5_Load(object sender, EventArgs e)
        {
            txtDirectorioModulos.Text = Properties.Settings.Default.LastDirectorySelected;
        }
        private void TransformNetFrameworkToNetcore3_1(string directoryFiles)
        {
            var filesPage = Directory.EnumerateFiles(directoryFiles, "*Page.cs", SearchOption.AllDirectories);
            foreach (var fiPage in filesPage)
            {
                var sSourcePage = File.ReadAllText(fiPage);
                if (sSourcePage.Contains("#if COREFX"))
                {
                    continue;//quiere decir que ya se modifico
                }
                sSourcePage = TransformPage(sSourcePage);
                File.WriteAllText(fiPage, sSourcePage);
            }
            var filesEndPoint = Directory.EnumerateFiles(directoryFiles, "*EndPoint.cs", SearchOption.AllDirectories);
            foreach (var fiEndPoint in filesEndPoint)
            {
                var sSourceEndPoint = File.ReadAllText(fiEndPoint);
                if (sSourceEndPoint.Contains("#if COREFX"))
                {
                    continue;
                }
                sSourceEndPoint = TransformEndPoint(sSourceEndPoint);
                File.WriteAllText(fiEndPoint, sSourceEndPoint);
            }
        }
        private string TransformEndPoint(string sourceEndPoint)
        {
            var sSource = sourceEndPoint.Replace("using System.Web.Mvc;",
                    @"
#if COREFX
	using Microsoft.AspNetCore.Mvc;
#else
	using System.Web.Mvc;
#endif");
            var pattern = @" *\[ *RoutePrefix\(\""([\w/]+)\""\) *\, *Route\(\""\{([\w=]+)\}""\)\]";
            sSource = Regex.Replace(sSource, pattern, @"#if COREFX
    [Route(""$1/[$2]"")]
#else
    [RoutePrefix(""$1""), Route(""{action}"")]
#endif");
            return sSource;
        }
        private string TransformPage(string sourcePage)
        {
            var sSource = sourcePage.Replace("using System.Web.Mvc;",
                    @"
#if COREFX
	using Microsoft.AspNetCore.Mvc;
#else
	using System.Web.Mvc;
#endif");

            var pattern = @" *\[ *RoutePrefix\((\""[\w/]+\"")\) *\, *Route\(\""\{([\w=]+)\}""\)\]";
            MatchCollection matches = Regex.Matches(sSource, pattern);
            var sRoutePrefix = "";
            foreach (Match match in matches)
            {
                sRoutePrefix = match.Groups[1].Value;
            }
            sSource = Regex.Replace(sSource, pattern, "#if !COREFX" + Environment.NewLine +
                                                                            "    [RoutePrefix($1), Route(\"{$2}\")]" + Environment.NewLine +
                                                                            "#endif");
            pattern = @"(public *ActionResult *Index\(\))";
            sSource = Regex.Replace(sSource, pattern, @"#if COREFX
        [Route(" + sRoutePrefix + @")]
#endif
        $1");
            return sSource;
        }
    }
}
