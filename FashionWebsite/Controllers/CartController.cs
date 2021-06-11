using FashionWebsite.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionWebsite.Controllers
{
    public class CartController : Controller
    {
        WebShoseEntities context = new WebShoseEntities();

        // GET: Cartc
        public ActionResult AddItemcart(int id)
        {
            PRODUCT pRODUCT= context.PRODUCTs.Find(id);
            if(pRODUCT==null)
            {
                return null;
            }
            else
            {
                MEMBER mEMBER = Session["MEMBER"] as MEMBER;
                CART itemCart = new CART();
                itemCart.MEMBERID = mEMBER.ID;
                itemCart.PRODUCTID = id;
                itemCart.QUANTITY = 1;
                itemCart.PRICE = pRODUCT.PROMOTIONPRICE;
                itemCart.TOTAL = itemCart.QUANTITY * itemCart.PRICE;
                itemCart.IMAGE = pRODUCT.IMAGE1;
                itemCart.NAME = pRODUCT.NAME;
                context.CARTs.Add(itemCart);
                context.SaveChanges();
                List<CART> cARTs = GetCart();
            }
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public ActionResult CheckQuantityAdd(int ID)
        {
            //Kiểm tra sản phẩm tồn tại hay không
            PRODUCT product = context.PRODUCTs.Find(ID);
            if (product == null)
            {
                //product does not exist
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng
            List<CART> listCart = GetCart();
            //Kiểm tra số lượng
            if (listCart != null)
            {
                int sum = 0;
                foreach (CART item in listCart.Where(x => x.PRODUCTID == ID))
                {
                    sum += item.QUANTITY.Value;
                }
                if (product.QUANTITY > sum)
                {
                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
            else
            {
                if (product.QUANTITY > 0)
                {

                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
        }
        [HttpPost]
        public ActionResult CheckQuantityUpdate(int ID, int Quantity)
        {
            //Kiểm tra sản phẩm tồn tại hay không
            PRODUCT product = context.PRODUCTs.Find(ID);
            if (product.QUANTITY >= Quantity)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
        public List<CART> GetCart()
        {
            MEMBER member = Session["MEMBER"] as MEMBER;
            if (member != null)
            {
                if (context.CARTs.Where(x => x.MEMBERID == member.ID).Count() > 0)
                {
                    List<CART> carts = context.CARTs.Where(x => x.MEMBERID == member.ID).ToList();
                    Session["Cart"] = carts;
                    return carts;
                }
            }
            else
            {
                List<CART> listCart = Session["CART"] as List<CART>;
                //Check null session Cart
                if (listCart == null)
                {
                    //Initialization listCart
                    listCart = new List<CART>();
                    Session["CART"] = listCart;
                    return listCart;
                }
                return listCart;
            }
            return null;
        }
        [HttpGet]
        public ActionResult CheckOut()
        {
            List<CART> cARTs = GetCart();
            int total = (int)cARTs.Sum(x => x.QUANTITY);
            decimal Tong = (decimal)cARTs.Sum(x => x.TOTAL);
            ViewBag.Tong = Tong.ToString("#,##");
            ViewBag.Total = total;
            return View();
        }
        [HttpGet]
        public ActionResult RemoveItemCart(int id)
        {
            CART cART = context.CARTs.Find(id);
            context.CARTs.Remove(cART);
            context.SaveChanges();
            List<CART> cARTs = GetCart();
            return RedirectToAction("CheckOut");
        }
        [HttpPost]
        public ActionResult EditCart(int ID, int Quantity)
        {
            //Check stock quantity
            PRODUCT product = context.PRODUCTs.Find(ID);
            //Updated quantity in cart Session
            List<CART> listCart = GetCart();
            //Get products from within listCart to update
            CART itemCartUpdate = listCart.Find(n => n.PRODUCTID == ID);
            itemCartUpdate.QUANTITY = Quantity;
            itemCartUpdate.TOTAL = itemCartUpdate.QUANTITY * itemCartUpdate.PRICE;

            MEMBER member = Session["MEMBER"] as MEMBER;
            if (member != null)
            {
                //Update Cart Quantity Member
                CART cartUpdate = context.CARTs.Single(x => x.PRODUCTID == product.ID && x.MEMBERID == member.ID);
                cartUpdate.QUANTITY = Quantity;
                cartUpdate.TOTAL = cartUpdate.QUANTITY * cartUpdate.PRICE;
                context.SaveChanges();
                Session["CART"] = listCart;
            }

            return RedirectToAction("Checkout");
        }
        //public void AddQuantity(int ProductID)
        //{
        //    MEMBER mEMBER = Session["MEMBER"] as MEMBER;
        //    CART itemCart = context.CARTs.SingleOrDefault(x => x.MEMBERID == mEMBER.ID && x.PRODUCTID == ProductID);
        //    itemCart.QUANTITY++;
        //    context.SaveChanges();
        //}
    }
}