using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBT.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace MvcBT.Controllers
{
    public class AdminController : Controller
    {
        dbQLBanSachDataContext db = new dbQLBanSachDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult Sach()
        //{
        //    return View(db.SACHes.ToList());
        //}
        public ActionResult Sach(int ? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Themmoisach()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Xoasach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if(sach==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult Themmoisach(SACH sach, HttpPostedFileBase fileupload)
        {
            
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if(fileupload==null)
            {
                ViewBag.Thongbao = "Vui lòng chọn hình minh họa";
                return View();
            }
            else
            {
                if(ModelState.IsValid)
                {
                    //Luu ten file, them thu vien using System.IO;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan file
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    //Kiem tra hinh anh ton tai
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                            fileupload.SaveAs(path);
                    }
                    sach.Hinhminhhoa = fileName;
                    db.SACHes.InsertOnSubmit(sach);
                    db.SubmitChanges();
                }
                return RedirectToAction("Sach");
            }

            //return View();
        }
        public ActionResult Chitietsach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if(sach==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
       [HttpPost,ActionName("Xoasach")]
       public ActionResult Xacnhanxoa(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if(sach==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SACHes.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("Sach");
        }
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            if(sach==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);  
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileupload)
        {
            ViewBag.Masach = sach.Masach;
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            SACH s = db.SACHes.ToList().Find(n => n.Masach == sach.Masach);

            if (ModelState.IsValid)
            {
                if (fileupload != null)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileupload.SaveAs(path);
                        s.Hinhminhhoa = fileName;
                    }
                }
                s.Masach = sach.Masach;
                s.Tensach = sach.Tensach;
                s.Donvitinh = sach.Donvitinh;
                s.Dongia = sach.Dongia;
                s.Mota = sach.Mota;
                s.MaCD = sach.MaCD;
                s.MaNXB = sach.MaNXB;
                s.Ngaycapnhat = sach.Ngaycapnhat;
                s.Soluongban = sach.Soluongban;
                s.solanxem = sach.solanxem;
                s.moi = sach.moi;
                db.SubmitChanges();

            }
            return RedirectToAction("Sach");

        }
    }
}