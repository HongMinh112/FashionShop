using FashionWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionWebsite.Controllers
{
    public class ProductController : Controller
    {
        WebShoseEntities context = new WebShoseEntities();
        // GET: Product
        public ActionResult Men(int from = -1, int to = -1)
        {
            if (from != -1 && to != -1)
            {
                IEnumerable<PRODUCT> pRODUCTs = context.PRODUCTs.ToList().Where(x => x.ISACTIVE == true && x.PRICE >= from && x.PRICE <= to);
                ViewBag.From = from;
                ViewBag.To = to;
                return View(pRODUCTs);
            }
            IEnumerable<PRODUCT> pRODUCTs2 = context.PRODUCTs.ToList().Where(x=>x.ISACTIVE==true);
            return View(pRODUCTs2);
        }
        public ActionResult Discount()
        {
            IEnumerable<PRODUCT> pRODUCTs = context.PRODUCTs.Where(x => x.DISCOUNT > 0);
            return View(pRODUCTs);
        }
        [HttpGet]
        public ActionResult Detail(int ID)
        {
            PRODUCT pRODUCTs = context.PRODUCTs.Find(ID);
            return View(pRODUCTs);
        }
        public ActionResult CartProduct()
        {
            return View();
        }
        public ActionResult Productcategory(int ID)
        {
            IEnumerable<PRODUCT> pRODUCTs = context.PRODUCTs.Where(x => x.CATEGORYID == ID && x.ISACTIVE==true);
            return View(pRODUCTs);
        }
        public ActionResult Search(string keyword)
        {
            IEnumerable<PRODUCT> pRODUCTs = context.PRODUCTs.Where(x => x.NAME.Contains(keyword));
            return View(pRODUCTs);
        }
    }
}