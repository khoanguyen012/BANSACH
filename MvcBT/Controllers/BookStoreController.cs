using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBT.Models;
using PagedList;
using PagedList.Mvc;
namespace MvcBT.Controllers
{
    public class BookStoreController : Controller
    {
        dbQLBanSachDataContext data = new dbQLBanSachDataContext();
        // GET: BookStore
        private List<SACH> Laysachmoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        //public ActionResult Index()
        //{
        //    var sachmoi = Laysachmoi(5);
        //    return View(sachmoi);
        //}
        public ActionResult Index(int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var sachmoi = Laysachmoi(15);
            return View(sachmoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Laychude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult Nhaxuatban()
        {
            var n = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(n);
        }
        public ActionResult SPTheoChude(int id)
        {
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return View(sach);
        }
        public ActionResult SPTheoNXB(int id)
        {
            var sach = from s in data.SACHes where s.MaNXB == id select s;
            return View(sach);
        }
        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes where s.Masach == id select s;
            return View(sach.Single());
        }
    }
}