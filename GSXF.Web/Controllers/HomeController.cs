using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSXF.Auxiliary;
using Newtonsoft.Json;
using GSXF.DataBase;
using GSXF.Model;
using GSXF.Security;
using System.Drawing;


namespace GSXF.Web.Controllers
{
    
    public class HomeController : BaseController
    {


        public ActionResult Index()
        {
            if(Request.Cookies["MyCookie"]!= null)
            {
                string username = Request.Cookies["MyCookie"]["username"];
                User user = userManager.Find(u => u.Name == username);

                if(user != null)
                {
                    user.IsOnline = false;
                    userManager.Update(user);
                }
            }
          

            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult JGCX()
        {
            return View();
        }

        public ActionResult XMCX()
        {
            return View();
        }
        public ActionResult GCSCX()
        {
            return View();
        }

        public ActionResult FWPJ()
        {
            return View();
        }



        [HttpGet]
        public ActionResult test()
        {
            return View();
        }


        public ActionResult Report(string filecode)
        {

            var project = projectManager.Find(p => p.ReportFileCode == filecode);
            UploadFile reportFile = project == null ? null : project.ReportFile;
            if (project == null || reportFile==null)
            {
                return Content("找不到此报告");
            }
            else
            {   
                return Redirect(reportFile.Path.Substring(1, reportFile.Path.Length - 1));
            }

        }



        [HttpPost]
        public ActionResult test(FormCollection form)
        {
            if(Request.Files.Count == 0)
            {
                return View();
            }
            var file = Request.Files[0];
            if(file.ContentLength == 0)
            {
                return View();
            }
            var fileName = file.FileName;
            
            var filePath = Server.MapPath(string.Format("~/{0}", "Upload"));

            var path = string.Format(filePath + "\\{0}", fileName);
            file.SaveAs(path);
            return View();
        }

        public ActionResult FeedBack(int projectID, string c)
        {

            var project = projectManager.Find(projectID);
            if (project == null)
            {
                return Content("不是有效的地址");
            }

            if (project.IsFeedBack)
            {
                return Content("已对此项目做出过评价，页面已失效");
            }
            //if (c != Encryption.SHA256(projectID.ToString()))
            //{
            //    return Content("不是有效的地址");
            //}


            ViewBag.Company = project.Company.Name;
            ViewBag.Name = project.Name;
            ViewBag.ID = project.ID;
            return View();
        }

        [HttpPost]
        public ActionResult FeedBack()
        {
            string id = Request.Form["projectID"];
            string score = Request.Form["Score"];
            string note = Request.Form["Note"];

            int projectID = int.Parse(id);
            int Score = int.Parse(score);

            var project = projectManager.Find(projectID);
            




            var company = project.Company;

            int count = company.Projects.Where(p => p.IsFeedBack == true).Count() + 1;
            int s = company.Score;
            company.Score = (company.Score + Score) / count;
            
            
            companyManager.Update(company);

            


            Evaluation eva = new Evaluation();
            eva.Project = project;
            eva.Source = EvaluationSource.客户评价;
            eva.Note = note;
            eva.Result = company.Score - s;
            evaluationManager.Add(eva);



            project.IsFeedBack = true;
            projectManager.Update(project);
            return View("Thanks");
        }

        public ActionResult Thanks()
        {
            return View();
        }

        public ActionResult LastArticleList(int category)
        {
            
            Category c = (Category)category;

            var articles = articleManager.FindList().Where(a => a.Category == c).OrderByDescending(a => a.ID).Take(5).ToList();
            if(articles.Count() < 5)
            {
                int count = articles.Count()+1;
                for (; count<=5; count++)
                {
                    articles.Add(null);
                }
            }
            ViewBag.articles = articles;
            return View();
        }

        public ActionResult ArticleList(int category)
        {
            Category c = (Category)category;

            var articles = articleManager.FindList().Where(a => a.Category == c).OrderByDescending(a => a.ID).ToList();
            ViewBag.articles = articles;
            ViewBag.Category = c.ToString();

            return View();
        }



        [AllowAnonymous]
        public ActionResult VerificationCode()
        {
            int _verificationLength = 6;
            int _width = 100, _height = 20;
            SizeF _verificationTextSize;
            Bitmap _bitmap = new Bitmap(Server.MapPath("~/Content/img/Texture.jpg"), true);
            TextureBrush _brush = new TextureBrush(_bitmap);
            //获取验证码
            string _verificationText = Verification.Text(_verificationLength);
            //存储验证码
            Session["VerificationCode"] = _verificationText.ToUpper();
            Font _font = new Font("Arial", 14, FontStyle.Bold);
            Bitmap _image = new Bitmap(_width, _height);
            Graphics _g = Graphics.FromImage(_image);
            //清空背景色
            _g.Clear(Color.White);
            //绘制验证码
            _verificationTextSize = _g.MeasureString(_verificationText, _font);
            _g.DrawString(_verificationText, _font, _brush, (_width - _verificationTextSize.Width) / 2, (_height - _verificationTextSize.Height) / 2);
            _image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            return null;
        }


    }
}