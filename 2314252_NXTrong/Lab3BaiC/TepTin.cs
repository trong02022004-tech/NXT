using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3BaiC
{
    internal class TepTin
    {
        private string duongDan;

        public TepTin(string path)
        {
            duongDan = path;
            if (!File.Exists(duongDan))
            {
                File.Create(duongDan).Close();
            }
        }

        public List<SinhVien> DocDanhSach()
        {
            var ds = new List<SinhVien>();
            foreach (var dong in File.ReadAllLines(duongDan))
            {
                var sv = SinhVien.Parse(dong);
                if (sv != null) ds.Add(sv);
            }
            return ds;
        }

        public void GhiDanhSach(List<SinhVien> ds)
        {
            using (StreamWriter sw = new StreamWriter(duongDan))
            {
                foreach (var sv in ds)
                {
                    sw.WriteLine(sv.ToString());
                }
            }
        }
    }
}
