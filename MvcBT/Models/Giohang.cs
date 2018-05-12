using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBT.Models
{
    public class Giohang
    {
        dbQLBanSachDataContext data = new dbQLBanSachDataContext();
        public int iMasach { set; get; }
        public string sTensach { set; get; }
        public string sHinhminhoa { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }

        public Giohang(int Masach)
        {
            iMasach = Masach;
            SACH sach = data.SACHes.Single(n => n.Masach == iMasach);
            sTensach = sach.Tensach;
            sHinhminhoa = sach.Hinhminhhoa;
            dDongia = double.Parse(sach.Dongia.ToString());
            iSoluong = 1;
        }
    }
}