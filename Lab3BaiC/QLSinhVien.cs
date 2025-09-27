using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Lab3BaiC
{
    internal class QLSinhVien
    {
        private List<SinhVien> danhSach;
        private TepTin tep;

        public QLSinhVien(string filePath)
        {
            tep = new TepTin(filePath);
            danhSach = tep.DocDanhSach();
        }

        public List<SinhVien> LayTatCa()
        {
            return danhSach;
        }

        public void ThemHoacCapNhat(SinhVien sv)
        {
            var index = danhSach.FindIndex(s => s.MSSV == sv.MSSV);
            if (index >= 0)
                danhSach[index] = sv;
            else
                danhSach.Add(sv);

            tep.GhiDanhSach(danhSach);
        }

        public void Xoa(List<string> listMSSV)
        {
            danhSach.RemoveAll(sv => listMSSV.Contains(sv.MSSV));
            tep.GhiDanhSach(danhSach);
        }
    }

}
