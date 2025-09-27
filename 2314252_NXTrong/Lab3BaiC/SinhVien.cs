using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3BaiC
{
    internal class SinhVien
    {
        public string MSSV { get; set; }
        public string HoVaTenLot { get; set; }
        public string Ten { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Lop { get; set; }
        public string SoCMND { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public string GioiTinh { get; set; }
        public List<string> MonHoc { get; set; }

        public SinhVien()
        {
            MonHoc = new List<string>();
        }

        public override string ToString()
        {
            return $"{MSSV};{HoVaTenLot};{Ten};{NgaySinh:dd/MM/yyyy};{Lop};{SoCMND};{SoDienThoai};{DiaChi};{GioiTinh};{string.Join(",", MonHoc)}";
        }

        public static SinhVien Parse(string dong)
        {
            if (string.IsNullOrWhiteSpace(dong)) return null;
            var parts = dong.Split(';');
            if (parts.Length < 10) return null;

            DateTime ngaySinh;
            string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd", "MM/dd/yyyy" };

            if (!DateTime.TryParseExact(parts[3].Trim(),
                                        formats,
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None,
                                        out ngaySinh))
            {
                // Nếu vẫn không parse được => gán mặc định
                ngaySinh = DateTime.MinValue;
            }

            return new SinhVien
            {
                MSSV = parts[0].Trim(),
                HoVaTenLot = parts[1].Trim(),
                Ten = parts[2].Trim(),
                NgaySinh = ngaySinh,
                Lop = parts[4].Trim(),
                SoCMND = parts[5].Trim(),
                SoDienThoai = parts[6].Trim(),
                DiaChi = parts[7].Trim(),
                GioiTinh = parts[8].Trim(),
                MonHoc = parts[9].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(m => m.Trim())
                                .ToList()
            };
        }

    }
}
