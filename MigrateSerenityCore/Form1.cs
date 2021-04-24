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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnMigrar_Click(object sender, EventArgs e)
        {
            var diModules = new DirectoryInfo(txtDirectorioModulos.Text);
            if (!diModules.Exists) return;
            if (MessageBox.Show("Favor de realizar respaldo antes de continuar, ya que este proceso sobreescribira los archivos existentes,¿Confirma que desea continuar?",
                "Confirma que desea continuar con el Proceso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No) return;
            var filesPage = Directory.EnumerateFiles(diModules.FullName, "*Page.cs", SearchOption.AllDirectories);
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
            var filesEndPoint = Directory.EnumerateFiles(diModules.FullName, "*EndPoint.cs", SearchOption.AllDirectories);
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
            MessageBox.Show("Finalizo el proceso con exito", "Finalizo el Proceso con exito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog opfd = new FolderBrowserDialog();
            if (opfd.ShowDialog() != DialogResult.OK) return;
            txtDirectorioModulos.Text = opfd.SelectedPath;
        }
    }
}
