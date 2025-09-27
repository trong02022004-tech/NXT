using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3BaiC
{

    public partial class Form1 : Form
    {

        private List<SinhVien> danhSachSV = new List<SinhVien>();
        private QLSinhVien ql;
      

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rdNam.Checked = true;
            cboLop.SelectedIndex = 0;
            ql = new QLSinhVien("data.txt");
            dgvDSSV.AutoGenerateColumns = false;
            dgvDSSV.Columns.Clear();

            dgvDSSV.Columns.Add("MSSV", "MSSV");
            dgvDSSV.Columns.Add("HoVaTenLot", "Họ và tên lót");
            dgvDSSV.Columns.Add("Ten", "Tên");
            dgvDSSV.Columns.Add("NgaySinh", "Ngày sinh");
            dgvDSSV.Columns.Add("Lop", "Lớp");
            dgvDSSV.Columns.Add("SoCMND", "Số CMND");
            dgvDSSV.Columns.Add("SoDienThoai", "Số điện thoại");
            dgvDSSV.Columns.Add("DiaChi", "Địa chỉ");
            HienThiDanhSach(ql.LayTatCa());
            
        }
        private List<string> LayMonHoc()
        {
            return clbMonHoc.CheckedItems.Cast<string>().ToList();
        }
        private void HienThiDanhSach(List<SinhVien> ds)
        {
            dgvDSSV.Rows.Clear();
            foreach (var sv in ds)
            {
                dgvDSSV.Rows.Add(
                    sv.MSSV,
                    sv.HoVaTenLot,
                    sv.Ten,
                    sv.NgaySinh.ToString("dd/MM/yyyy"),
                    sv.Lop,
                    sv.SoCMND,
                    sv.SoDienThoai,
                    sv.DiaChi
                );
            }
        }
        private SinhVien LayThongTinTuForm()
        {
            SinhVien sv = new SinhVien
            {
                MSSV = mtxtMaSo.Text,
                HoVaTenLot = txtHoTen.Text,   
                Ten = txtTen.Text,
                NgaySinh = dtpNgaySinh.Value,
                Lop = cboLop.Text,
                SoCMND = mtxtCMND.Text,
                SoDienThoai = mtxtSDT.Text,
                DiaChi = txtDiaChi.Text,
                GioiTinh = rdNam.Checked ? "Nam" : "Nữ",
                MonHoc = clbMonHoc.CheckedItems.Cast<string>().ToList()
            };
            return sv;
        }
       
        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            var sv = LayThongTinTuForm();
            if (string.IsNullOrWhiteSpace(sv.MSSV))
            {
                MessageBox.Show("MSSV không được để trống!");
                return;
            }

            ql.ThemHoacCapNhat(sv);
            HienThiDanhSach(ql.LayTatCa());
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (dgvDSSV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chọn sinh viên để cập nhật!");
                return;
            }

            var sv = LayThongTinTuForm();
            ql.ThemHoacCapNhat(sv);
            HienThiDanhSach(ql.LayTatCa());
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSSV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chọn sinh viên để xóa!");
                return;
            }

            var listMSSV = new List<string>();
            foreach (DataGridViewRow row in dgvDSSV.SelectedRows)
            {
                listMSSV.Add(row.Cells[0].Value.ToString());
            }

            ql.Xoa(listMSSV);
            HienThiDanhSach(ql.LayTatCa());
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();

            // Tìm theo MSSV, Tên, hoặc Lớp mà không cần cboTimKiem
            var ketQua = ql.LayTatCa()
                           .Where(sv => sv.MSSV.Contains(tuKhoa)
                                     || sv.Ten.Contains(tuKhoa)
                                     || sv.Lop.Contains(tuKhoa))
                           .ToList();

            HienThiDanhSach(ketQua);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
             if (MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void dgvDSSV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDSSV.SelectedRows.Count > 0)
            {
                var row = dgvDSSV.SelectedRows[0];
                mtxtMaSo.Text = row.Cells[0].Value.ToString();
                txtHoTen.Text = row.Cells[1].Value.ToString();
                txtTen.Text = row.Cells[2].Value.ToString();
                dtpNgaySinh.Value = DateTime.Parse(row.Cells[3].Value.ToString());
                cboLop.Text = row.Cells[4].Value.ToString();
                mtxtCMND.Text = row.Cells[5].Value.ToString();
                mtxtSDT.Text = row.Cells[6].Value.ToString();
                txtDiaChi.Text = row.Cells[7].Value.ToString();
                if (row.Cells[8].Value.ToString() == "Nam") rdNam.Checked = true;
                else rdNu.Checked = true;

                // Reset môn học
                for (int i = 0; i < clbMonHoc.Items.Count; i++)
                    clbMonHoc.SetItemChecked(i, false);

                // Check lại môn học
                var monHocDaChon = row.Cells[9].Value.ToString().Split(',');
                foreach (var mh in monHocDaChon)
                {
                    int index = clbMonHoc.Items.IndexOf(mh);
                    if (index >= 0) clbMonHoc.SetItemChecked(index, true);
                }
            }
        }

        private void dgvDSSV_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDSSV.SelectedRows.Count > 0)
            {
                var row = dgvDSSV.SelectedRows[0];
                mtxtMaSo.Text = row.Cells[0].Value.ToString();
                txtHoTen.Text = row.Cells[1].Value.ToString();
                txtTen.Text = row.Cells[2].Value.ToString();
                dtpNgaySinh.Value = DateTime.Parse(row.Cells[3].Value.ToString());
                cboLop.Text = row.Cells[4].Value.ToString();
                mtxtCMND.Text = row.Cells[5].Value.ToString();
                mtxtSDT.Text = row.Cells[6].Value.ToString();
                txtDiaChi.Text = row.Cells[7].Value.ToString();

                // Giới tính và Môn học lấy trực tiếp từ file (trong QLSinhVien / SinhVien)
                var sv = ql.LayTatCa().FirstOrDefault(s => s.MSSV == mtxtMaSo.Text);
                if (sv != null)
                {
                    if (sv.GioiTinh == "Nam") rdNam.Checked = true;
                    else rdNu.Checked = true;

                    // Reset các môn học
                    for (int i = 0; i < clbMonHoc.Items.Count; i++)
                        clbMonHoc.SetItemChecked(i, false);

                    // Check lại các môn học
                    foreach (var mh in sv.MonHoc)
                    {
                        int index = clbMonHoc.Items.IndexOf(mh);
                        if (index >= 0) clbMonHoc.SetItemChecked(index, true);
                    }
                }
            }
        }
    }
}
