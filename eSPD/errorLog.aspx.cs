using eSPD.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSPD
{
    public partial class errorLog : System.Web.UI.Page
    {
        string path = @"E:\SPD\ErrorMail\";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    if (!File.Exists(path))
                    {
                        (new FileInfo(path)).Directory.Create();
                    }

                    string[] filePaths = Directory.GetFiles(path);
                    List<ListItem> files = new List<ListItem>();
                    foreach (string filePath in filePaths)
                    {
                        files.Add(new ListItem(Path.GetFileName(filePath), filePath));
                    }

                    gridViewError.DataSource = files;
                    gridViewError.DataBind();
                }
                catch (Exception)
                {

                }
            }
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
        }

        protected void btnBreak_Click(object sender, EventArgs e)
        {
            ApplicationPoolRecycle.RecycleCurrentApplicationPool();
            HttpRuntime.UnloadAppDomain();
        }
    }
}