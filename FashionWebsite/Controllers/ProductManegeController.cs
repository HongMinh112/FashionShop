using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FashionWebsite.Models;
using System.Net;
using System.IO;
using System.Data.Entity;

namespace FashionWebsite.Controllers
{
    public class ProductManegeController : Controller
    {
        WebShoseEntities context = new WebShoseEntities();

        // GET: ProductManege
        public ActionResult List()
        {
            IEnumerable<PRODUCT> PRODUCTs = context.PRODUCTs.ToList();
            return View(PRODUCTs);
        }
        [HttpGet]
        public ActionResult Edit(int ID)
        {
            PRODUCT PRODUCTs = context.PRODUCTs.Find(ID);
            ViewBag.TRADEMARKID = new SelectList(context.TRADEMARKs.ToList(), "ID", "Name", PRODUCTs.TRADEMARKID);
            ViewBag.CATEGORYID = new SelectList(context.PRODUCTCATOGERies.ToList(), "ID", "Name", PRODUCTs.CATEGORYID);
            ViewBag.GENDERID = new SelectList(context.GENDERs.ToList(), "ID", "Name", PRODUCTs.GENDERID);
            return View(PRODUCTs);
        }
        [HttpPost]
        public ActionResult Edit(PRODUCT pRODUCT, HttpPostedFileBase[] ImageUpload)
        {
            //Khai báo một errorCount
            int errorCount = 0;
            for (int i = 0; i < ImageUpload.Length; i++)
            {
                //Kiểm tra nội dung hình ảnh
                if (ImageUpload[i] != null && ImageUpload[i].ContentLength > 0)
                {
                    //Kiểm tra định dạng hình ảnh
                    if (ImageUpload[i].ContentType != "image/jpeg" && ImageUpload[i].ContentType != "image/png" && ImageUpload[i].ContentType != "image/gif")
                    {
                        //Tăng lỗi
                        errorCount++;
                    }
                    else
                    {
                        //Nhận tên file
                        var fileName = Path.GetFileName(ImageUpload[i].FileName);
                        //Nhận đường dẫn
                        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                        //Kiểm tra tồn tại
                        if (!System.IO.File.Exists(path))
                        {
                            //Thêm hình ảnh vào thư mục
                            ImageUpload[i].SaveAs(path);
                        }
                    }
                }
            }
            //Đặt hình ảnh giá trị mới cho sản phẩm
            for (int i = 0; i < ImageUpload.Length; i++)
            {
                if (ImageUpload[i] != null)
                {
                    if (i == 0)
                        pRODUCT.IMAGE1 = ImageUpload[0].FileName;
                    else if (i == 1)
                        pRODUCT.IMAGE2 = ImageUpload[1].FileName;
                    else if (i == 2)
                        pRODUCT.IMAGE3 = ImageUpload[2].FileName;
                    else if (i == 3)
                        pRODUCT.IMAGE4 = ImageUpload[3].FileName;
                }
            }

            //Đặt TempData cho chế độ xem đăng ký để show swal
            TempData["edit"] = "Success";
            //Cập nhật sản phẩm
            PRODUCT productUpdate = context.PRODUCTs.Find(pRODUCT.ID);
            productUpdate.ID = pRODUCT.ID;
            productUpdate.NAME = pRODUCT.NAME;
            productUpdate.TRADEMARKID = pRODUCT.TRADEMARKID;
            productUpdate.GENDERID = pRODUCT.GENDERID;
            productUpdate.IMAGE1 = pRODUCT.IMAGE1;
            productUpdate.IMAGE2 = pRODUCT.IMAGE2;
            productUpdate.IMAGE3 = pRODUCT.IMAGE3;
            productUpdate.IMAGE4 = pRODUCT.IMAGE4;
            productUpdate.CATEGORYID = pRODUCT.CATEGORYID;
            productUpdate.COMMENTCOUNT = pRODUCT.COMMENTCOUNT;
            productUpdate.HOMEFLAG = pRODUCT.HOMEFLAG;
            productUpdate.HOTFLAG = pRODUCT.HOTFLAG;
            productUpdate.DESCRIPTION = pRODUCT.DESCRIPTION;
            productUpdate.DISCOUNT = pRODUCT.DISCOUNT;
            productUpdate.NAME = pRODUCT.NAME;
            productUpdate.ISACTIVE = pRODUCT.ISACTIVE;
            productUpdate.ISNEW = pRODUCT.ISNEW;
            productUpdate.LASTUPDATEDATE = pRODUCT.LASTUPDATEDATE;
            productUpdate.PRICE = pRODUCT.PRICE;
            decimal Pricee = (decimal)(productUpdate.PRICE - ((productUpdate.PRICE * productUpdate.DISCOUNT)) / 100);
            pRODUCT.PROMOTIONPRICE = Pricee;
            productUpdate.PROMOTIONPRICE = pRODUCT.PROMOTIONPRICE;
            productUpdate.PURCHASECOUNT = pRODUCT.PURCHASECOUNT;
            productUpdate.QUANTITY= pRODUCT.QUANTITY;
            productUpdate.VIEWCOUNT = pRODUCT.VIEWCOUNT;
            productUpdate.WARRANTYINFORMATION = pRODUCT.WARRANTYINFORMATION;
            context.SaveChanges();
            string Url = Request.Url.ToString();
            return RedirectToAction("List");
        }
        [HttpGet]
        public ActionResult Insert()
        {
            ViewBag.TRADEMARKID = new SelectList(context.TRADEMARKs.ToList(), "ID", "Name");
            ViewBag.CATEGORYID = new SelectList(context.PRODUCTCATOGERies.ToList(), "ID", "Name");
            ViewBag.GENDERID = new SelectList(context.GENDERs.ToList(), "ID", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Insert(PRODUCT pRODUCT, HttpPostedFileBase[] ImageUpload)
        {
            //Khai báo một errorCount
            int errorCount = 0;
            for (int i = 0; i < ImageUpload.Length; i++)
            {
                //Kiểm tra nội dung hình ảnh
                if (ImageUpload[i] != null && ImageUpload[i].ContentLength > 0)
                {
                    //Kiểm tra định dạng hình ảnh
                    if (ImageUpload[i].ContentType != "image/jpeg" && ImageUpload[i].ContentType != "image/png" && ImageUpload[i].ContentType != "image/gif")
                    {
                        //Tăng lỗi
                        errorCount++;
                    }
                    else
                    {
                        //Nhận tên file
                        var fileName = Path.GetFileName(ImageUpload[i].FileName);
                        //Nhận đường dẫn
                        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                        //Kiểm tra tồn tại
                        if (!System.IO.File.Exists(path))
                        {
                            //Thêm hình ảnh vào thư mục
                            ImageUpload[i].SaveAs(path);
                        }
                    }
                }
            }
            //Đặt hình ảnh giá trị mới cho sản phẩm
            for (int i = 0; i < ImageUpload.Length; i++)
            {
                if (ImageUpload[i] != null)
                {
                    if (i == 0)
                        pRODUCT.IMAGE1 = ImageUpload[0].FileName;
                    else if (i == 1)
                        pRODUCT.IMAGE2 = ImageUpload[1].FileName;
                    else if (i == 2)
                        pRODUCT.IMAGE3 = ImageUpload[2].FileName;
                    else if (i == 3)
                        pRODUCT.IMAGE4 = ImageUpload[3].FileName;
                }
            }

            //Đặt TempData cho chế độ xem đăng ký để show swal
            TempData["edit"] = "Success";
            pRODUCT.PROMOTIONPRICE= (decimal)(pRODUCT.PRICE - ((pRODUCT.PRICE * pRODUCT.DISCOUNT)) / 100);
            context.PRODUCTs.Add(pRODUCT);
            context.SaveChanges();
            string Url = Request.Url.ToString();
            return RedirectToAction("List");
        }
        //public List<CART> GetProduct()
        //{
        //    MEMBER member = Session["MEMBER"] as MEMBER;
        //    if (member != null)
        //    {
        //        if (context.CARTs.Where(x => x.MEMBERID == member.ID).Count() > 0)
        //        {
        //            List<CART> carts = context.CARTs.Where(x => x.MEMBERID == member.ID).ToList();
        //            Session["Cart"] = carts;
        //            return carts;
        //        }
        //    }
        //    else
        //    {
        //        List<CART> listCart = Session["CART"] as List<CART>;
        //        //Check null session Cart
        //        if (listCart == null)
        //        {
        //            //Initialization listCart
        //            listCart = new List<CART>();
        //            Session["CART"] = listCart;
        //            return listCart;
        //        }
        //        return listCart;
        //    }
        //    return null;
        //}
        public ActionResult Block (int ID)
        {
            PRODUCT pRODUCT = context.PRODUCTs.Single(x=>x.ID==ID);
            pRODUCT.ISACTIVE = false;
            context.SaveChanges();
            return RedirectToAction("List");
        }
        public ActionResult Active(int ID)
        {
            PRODUCT pRODUCT = context.PRODUCTs.Single(x => x.ID == ID);
            pRODUCT.ISACTIVE = true;
            context.SaveChanges();
            return RedirectToAction("List");
        }
    }
}