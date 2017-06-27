using System;
using System.Web.Mvc;
using GSXF.DataBase;
using GSXF.Model;
using GSXF.Web.Models;
using GSXF.Auxiliary;
using GSXF.Security;
using System.Collections.Generic;
using System.Linq;
//using Spire.Pdf;
//using Spire.Pdf.Graphics;
using System.IO;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net.Mail;
using System.Net;

using iTextSharp.text;
using iTextSharp.text.pdf;


namespace GSXF.Web.Controllers
{

    
    [UserAuthorize]
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            var user = Session["User"] as User;
            string roleName = user.Role.Name;
            ViewBag.User = user;
            ViewBag.OrgName = userManager.getOrgName(user.ID);
            return View();
        }


        public ActionResult Menu()
        {
            User user = Session["User"] as User;
            var roleName = user.Role.Name;
            ViewBag.Role = roleName;

            //标志是否为主账号
            ViewBag.Flag = user.Name.Substring(user.Name.Length - 2, 2) == "00";

            return PartialView();
        }

        public ActionResult ZZHGL()
        {
            return View();
        }

        public ActionResult MyInfo()
        {
            User user = Session["User"] as User;
            ViewBag.Name = user.Name;
            ViewBag.Regtime = user.RegTime;
            ViewBag.LoginTime = user.LoginTime;
            ViewBag.LoginIP = user.LoginIP;
            ViewBag.Role = user.Role.Name;

            ViewBag.CompanyName = userManager.getOrgName(user.ID);
            return View();
        }

        




        #region 超级管理员

        public ActionResult Index1()
        {
            return View();
        }
        #endregion

        #region 消防机构总队
        //总队首页
        public ActionResult Index2()
        {
            return View();
        }
        //项目监管
        public ActionResult XMJG()
        {
            User user = Session["User"] as User;
            ViewBag.Role = user == null ? "" : user.Role.Name;

            return View();
        }
        //机构备案
        public ActionResult JGBA()
        {

            return View();
        }
        //注册工程师备案
        public ActionResult ZCGCSBA()
        {
            return View();
        }
        //项目查询
        public ActionResult XMCX()
        {
            User user = Session["User"] as User;
            ViewBag.Role = user == null ? "" : user.Role.Name;

            return View();
        }
        //机构查询
        public ActionResult JGCX()
        {
            return View();
        }
        //注册工程师查询
        public ActionResult ZCGCSCX()
        {
            return View();
        }

        public ActionResult WZGL()
        {
            return View();
        }

        public ActionResult ZHGL()
        {
            return View();
        }

        #endregion

        #region 消防机构支队、大队
        public ActionResult Index3()
        {
            return View();
        }

        public ActionResult Index4()
        {
            return View();
        }

        //项目抽检
        public ActionResult XMCJ()
        {
            return View();
        }

        #endregion

        #region 服务机构

        public ActionResult Index5()
        {
            return View("XMJG");
        }

        public ActionResult XMDJ()
        {
            User user = Session["User"] as User;
            int companyID = userCompanyManager.Find(uc => uc.UserID == user.ID).CompanyID;
            Company company = companyManager.Find(companyID);
            int count = company.Employees.Where(e => e.Level == EmployeeLevel.临时注册消防工程师 || e.Level == EmployeeLevel.一级注册消防工程师 || e.Level == EmployeeLevel.二级注册消防工程师).Count();
            //if(count < 8)
            //{
            //    return Content("检测到注册消防工程师少于8人，该功能已被暂停使用...");
            //}
            

            return View();
        }

        public ActionResult RYBA()
        {
            return View();
        }
        #endregion


        //项目详情
        public ActionResult XMXQ(int projectID)
        {
            var user = Session["User"] as User;
            ViewBag.Role = user == null ? "" : user.Role.Name;
            ViewBag.Progress = (int)projectManager.Find(projectID).Progress;

            ViewBag.Path = "/Data/getProject/?projectID=" + projectID.ToString();
            ViewBag.ID = projectID;
            return View();
        }

        public ActionResult JGXQ(int companyID)
        {
            ViewBag.companyID = companyID;
            return View();
        }


        [HttpPost]
        public ActionResult xmcj()
        {
            string id = Request.Form["projectID"];
            string result = Request.Form["Result"];
            string note = Request.Form["Note"];

            int projectID = int.Parse(id);
            var project = projectManager.Find(projectID);
            var company = project.Company;


            Evaluation eva = new Evaluation();
            eva.Project = project;
            eva.Source = EvaluationSource.项目抽查;
            eva.Note = note;
            




            if (result == "0")
            {
                eva.Result = 0;
            }
            else
            {
                company.Score = company.Score - 5;
                companyManager.Update(company);
                eva.Result = -5;
            }
            evaluationManager.Add(eva);


            projectManager.Update(project);
            return Json("123");
        }


        [HttpPost]
        public ActionResult rcjc()
        {
            Response resp = new Response();

            

            string id = Request.Form["id"];
            string date = Request.Form["date"];

            int projectID = int.Parse(id);
            Project project = projectManager.Find(projectID);

            project.CheckTime = DateTime.Parse(date);
            project.Progress = ProjectProgress.入场检测;
            
            if (Request.Files.Count == 0)
            {
                resp.Code = -1;
                resp.Message = "上传文件不能为空";
                return Json(resp);
            }
            var file = Request.Files[0];
            if (file.ContentLength >= 20971520)
            {
                resp.Code = -2;
                resp.Message = "上传文件太大";
                return Json(resp);
            }
            var fileName = file.FileName;

            if (fileName.Substring(fileName.Length - 4, 4) != ".rar")
            {
                resp.Code = -3;
                resp.Message = "文件格式不符合要求";
                return Json(resp);
            }

            fileName = Verification.Text(20) + ".rar";

            var filePath = Server.MapPath("~/Upload/Datas");

            var path = string.Format(filePath + "\\{0}", fileName);
            file.SaveAs(path);


            Model.UploadFile f = new Model.UploadFile();
            f.Name = fileName;
            f.Path = "~/Upload/Datas/" + fileName;
            var res = fileManager.Add(f);
            project.DataFile = f;
            projectManager.Update(project);
            return Json(resp);
        }

        [HttpPost]
        public ActionResult cjbg()
        {
            Response resp = new Response();



            string id = Request.Form["id"];
            int projectID = int.Parse(id);
            Project project = projectManager.Find(projectID);

            

            if (Request.Files.Count == 0)
            {
                resp.Code = -1;
                resp.Message = "上传文件不能为空";
                return Json(resp);
            }
            var file = Request.Files[0];
            if (file.ContentLength >= 20971520)
            {
                resp.Code = -2;
                resp.Message = "上传文件太大";
                return Json(resp);
            }
            var fileName = file.FileName;

            if (fileName.Substring(fileName.Length - 4, 4) != ".pdf")
            {
                resp.Code = -3;
                resp.Message = "文件格式不符合要求";
                return Json(resp);
            }

            
            var filePath = Server.MapPath("~/Upload/Reports");

            var path = string.Format(filePath + "\\{0}", fileName);
            file.SaveAs(path);
            
            fileName = Verification.Text(20) + ".pdf";




            string inputfilepath = path;
            string outputfilepath = string.Format(filePath + "\\{0}", fileName);
            string waterMarkName = "查询码："+ project.ReportFileCode;
            string pngpath = Server.MapPath("~/Content/img/waterprint_09.png");
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            try
            {
                pdfReader = new PdfReader(inputfilepath);
                pdfStamper = new PdfStamper(pdfReader, new FileStream(outputfilepath, FileMode.Create));
                int total = pdfReader.NumberOfPages + 1;
                iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);
                float width = psize.Width;
                float height = psize.Height;
                PdfContentByte content;
                BaseFont font = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\SIMFANG.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                PdfGState gs = new PdfGState();

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(pngpath);

                image.SetAbsolutePosition(width / 2 - image.Width / 2, height / 2 - image.Height / 2);

                for (int i = 1; i < total; i++)
                {
                    content = pdfStamper.GetUnderContent(i);//在内容上方加水印
                    //content = pdfStamper.GetUnderContent(i);//在内容下方加水印
                    //透明度
                    content.AddImage(image);
                    content.SetGState(gs);
                    //content.SetGrayFill(0.3f);
                    //开始写入文本
                    content.BeginText();
                    content.SetColorFill(BaseColor.BLUE);
                    content.SetFontAndSize(font, width / 40);
                    content.SetTextMatrix(0, 0);
                    content.ShowTextAligned(Element.ALIGN_RIGHT, waterMarkName, width, height - height / 40, 0);
                    //content.SetColorFill(BaseColor.BLACK);
                    //content.SetFontAndSize(font, 8);
                    //content.ShowTextAligned(Element.ALIGN_CENTER, waterMarkName, 0, 0, 0);
                    content.EndText();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                if (pdfStamper != null)
                    pdfStamper.Close();

                if (pdfReader != null)
                    pdfReader.Close();
            }

            System.IO.File.Delete(inputfilepath);


            Model.UploadFile f = new Model.UploadFile();
            f.Name = fileName;
            f.Path = "~/Upload/Reports/" + f.Name;
            var res = fileManager.Add(f);
            project.ReportFile = f;


            project.Progress = ProjectProgress.出具报告;
            projectManager.Update(project);
            return Json(resp);
        }
        [HttpPost]
        public ActionResult tjba()
        {
            string id = Request.Form["id"];
            int pid = int.Parse(id);

            Project project = projectManager.Find(pid);
            string result = Request.Form["result"];
            bool res = result == "0" ? true : false;
            project.Result = res;
            project.Progress = ProjectProgress.提交备案;
            project.RecordDate = DateTime.Now;

            projectManager.Update(project);

            
            string feedPath = DataController.getSiteConfig("SiteUrl") + "/Home/FeedBack/" + "?projectID=" + project.ID + "&&c=" + Security.Encryption.SHA256(project.ID.ToString());

            sendFeedEmail(project.ObjectEmail, feedPath,project.Company.Name, project.Name);
            return Content("成功");
        }

        private void sendFeedEmail(string to, string path,string companyName, string projectName)
        {
            MailMessage message = new MailMessage();
            string email = DataController.getSiteConfig("EmailName");
            string password = DataController.getSiteConfig("EmailPassword");


            MailAddress from = new MailAddress(email);
            message.From = from;
            message.To.Add(to);
            message.Subject = "项目评价";

            string content = "客户您好： 请您抽空对【" + companyName + "】公司的【" + projectName + "】项目服务做出评价。点击 " + path + " 进行评价。 ";

            message.Body = content;
            message.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.163.com";
            smtp.Credentials = new NetworkCredential(email,password);
            smtp.EnableSsl = true;
            smtp.Send(message);
        }
    }
}