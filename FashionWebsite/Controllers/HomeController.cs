using FashionWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace FashionWebsite.Controllers
{
    public class HomeController : Controller
    {
        //kết nối database
        WebShoseEntities context = new WebShoseEntities();
        [HttpGet]
        public ActionResult Index()
        {
            //ds sản phẩm nam và nữ
            IEnumerable<PRODUCT> menproducts = context.PRODUCTs.Where(x => x.GENDERID == 1 && x.ISACTIVE==true).ToList();
            IEnumerable<PRODUCT> womenproducts = context.PRODUCTs.Where(x => x.GENDERID == 2 && x.ISACTIVE==true).ToList();
            ViewBag.menproducts = menproducts;
            ViewBag.womenproducts = womenproducts;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string type)
        {
            string message = string.Empty;
            if (type == "nhapdung")
            {
                message = "sucsess message";
            }
            else if (type == "nhapsai")
            {
                message = "warning message";
            }
            else if (type == "chuanhapten")
            {
                message = "danger message";
            }
            else
            {
                message = "Nothing";
            }

            SetAlert(message, type);

            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        public ActionResult MenuPartial()
        {
            IEnumerable<PRODUCTCATOGERY> pRODUCTCATEGORY = context.PRODUCTCATOGERies;
            ViewBag.PRODUCTCATOGERY = pRODUCTCATEGORY;
            IEnumerable<SUBCATEGORY> sUBCATEGORY = context.SUBCATEGORies;
            ViewBag.SUBCATEGORY = sUBCATEGORY;
            if (Session["MEMBER"] != null)
            {

                List<CART> cARTs = Session["CART"] as List<CART>;
                if(cARTs!=null)
                {
                    ViewBag.totalquantity = cARTs.Sum(x => x.QUANTITY);
                    decimal Tong = (decimal)cARTs.Sum(x => x.TOTAL);
                    ViewBag.totalprice = Tong.ToString("#,##");
                }   
                else
                {
                ViewBag.totalquantity = 0;
                ViewBag.totalprice =0;
                }    
            }
            return View();
        }
        [HttpPost]
        //đăng kí
        public ActionResult SignUp(MEMBER mEMBER)
        {
            mEMBER.MEMBERCATEGORYID = 1;
            mEMBER.EMAILCONFIGMER = false;
            string Str = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random rd = new Random();
            int Randomcharindex = 0;
            char CharIndex;
            string capcha = "";
            for (int i = 0; i < 5; i++)
            {
                Randomcharindex = rd.Next(0, Str.Length);
                CharIndex = Str[Randomcharindex];
                capcha += Convert.ToString(CharIndex);
            }
            mEMBER.CAPCHA = capcha;
            context.MEMBERs.Add(mEMBER);
            context.SaveChanges();
            SentMail("Mã xác minh tài khoản ML cute", mEMBER.EMAIL, "minhbakaluffy114@gmail.com", "Minhbaka1", "<p>Mã xác minh của bạn: " + capcha + "<br/>Hoặc xác minh nhanh bằng cách click vào link: </p>");
            return RedirectToAction("ConfirmEmail", new { ID = mEMBER.ID });
        }
        //
        public void SentMail(string Title, string ToEmail, string FromEmail, string Password, string Content)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail);
            mail.From = new MailAddress(ToEmail);
            mail.Subject = Title;
            mail.Body = Content;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(FromEmail, Password);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
        [HttpGet]
        public ActionResult ConfirmEmail(int ID)
        {
            if (context.MEMBERs.Find(ID).EMAILCONFIGMER == true)
            {
                ViewBag.Message = "EmailConfirmed";
                return View();
            }
            ViewBag.ID = ID;
            string email = context.MEMBERs.Find(ID).EMAIL;
            ViewBag.Email = "Mã xác minh đã được gửi đến " + email;
            return View();
        }
        [HttpPost]
        public ActionResult ConfirmEmail(int ID, string CAPCHA)
        {
            MEMBER mEMBER = context.MEMBERs.Find(ID);
            if (mEMBER.CAPCHA == CAPCHA)
            {
                mEMBER.EMAILCONFIGMER = true;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("ConfirmEmail", new { ID = ID });
        }
        [HttpPost]
        //đăng nhập
        public ActionResult SignIn(MEMBER member)
        {
            if (member == null)
            {
            //    ModelState.AddModelError("", "Đăng nhập không thành công");
                return null;
            }
            else
            {
                try
                {
                    MEMBER memberCheck = context.MEMBERs.Single(x => x.FULLNAME == member.FULLNAME && x.PASSWORD == member.PASSWORD);
                    Session["MEMBER"] = memberCheck;
                    if (context.CARTs.Where(x => x.MEMBERID == memberCheck.ID).Count() > 0)
                    {
                        List<CART> carts = context.CARTs.Where(x => x.MEMBERID == memberCheck.ID).ToList();
                        Session["CART"] = carts;
                        SetAlert("Đăng nhập thành công", "nhapdung");
                        return RedirectToAction("Index");
                    }
                    {
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception)
                {
                    if (context.CARTs.Where(x => x.MEMBERID == member.ID).Count() > 0)
                    {
                        List<CART> carts = context.CARTs.Where(x => x.MEMBERID == member.ID).ToList();
                        Session["CART"] = carts;
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index");
                }
            }
        }
        public ActionResult LogOut()
        {
            Session["MEMBER"] = null;
            Session["CART"] = null;
            return RedirectToAction("Index");
        }
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "nhapdung")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "nhapsai")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "chuanhapten")
            {
                TempData["AlertType"] = "alert-danger";
            }
            else
            {
                TempData["AlertType"] = "alert-info";
            }
        }
    }
}