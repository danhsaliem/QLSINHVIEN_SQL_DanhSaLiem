using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace DanhSaLiem_QLSV
{
    public partial class Home : Form
    {
        SqlConnection conn = new SqlConnection("Server=LAPTOP-7KQ5GTV8;Database=QLSV;User Id=sa;Password=sa;");

        public Home()
        {
            InitializeComponent();

           
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMSV.Text))
            {
                MessageBox.Show("Vui lòng nhập MSSV!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(txtMSV.Text, @"^\d{10}$"))
            {
                MessageBox.Show("MSSV phải có 10 chữ số và không được chứa chữ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            if (string.IsNullOrWhiteSpace(txtTenSV.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (txtTenSV.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Tên sinh viên không được chứa số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtTenSV.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (txtTenSV.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Tên sinh viên không được chứa số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDiem.Text))
            {
                MessageBox.Show("Vui lòng nhập điểm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!double.TryParse(txtDiem.Text, out double diem) || diem < 0 || diem > 10)
            {
                MessageBox.Show("Điểm phải nằm trong khoảng từ 0 đến 10!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Openconn()
        {
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Closeconn()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        private bool Exe(string cmd)
        {
            Openconn();
            bool check;
            try
            {
                SqlCommand sc = new SqlCommand(cmd, conn);
                sc.ExecuteNonQuery();
                check = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                check = false;
            }
            finally
            {
                Closeconn();
            }
            return check;
        }

        private DataTable Red(string cmd)
        {
            Openconn();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand sc = new SqlCommand(cmd, conn);
                SqlDataAdapter sda = new SqlDataAdapter(sc);
                sda.Fill(dt);
            }
            catch (Exception)
            {
                dt = null;
                throw;
            }
            finally
            {
                Closeconn();
            }
            return dt;
        }

        private void load()
        {
            DataTable dt = Red("SELECT * FROM QLSINHVIEN");
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.ReadOnly = true;
            }
        }

        private void Home_Load(object sender, EventArgs e)
        {
            load();

        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            txtMSV.Clear();
            txtTenSV.Clear();
            txtNganh.Clear();
            txtLop.Clear();
            txtDiem.Clear();
            load();
            txtMSV.Enabled = true;
        }

        private void buttonThem_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            { 
                DataTable dt = Red($"SELECT * FROM QLSINHVIEN WHERE MASV = N'{txtMSV.Text}'");
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("MSSV đã tồn tại! Vui lòng nhập MSSV khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }
         
            
                Exe("INSERT INTO QLSINHVIEN(MASV, TenSV, Nganh, Lop, Diem) VALUES(N'" + txtMSV.Text + "', N'" + txtTenSV.Text + "', N'" + txtNganh.Text + "', N'" + txtLop.Text + "', N'" + txtDiem.Text + "')");
                load();
            }
        }

        private void buttonSua_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {

                Exe("UPDATE QLSINHVIEN SET TenSV=N'" + txtTenSV.Text + "', Nganh=N'" + txtNganh.Text + "', Lop=N'" + txtLop.Text + "', Diem=N'" + txtDiem.Text + "' WHERE MASV=N'" + txtMSV.Text + "'");
                load();
            }
          
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0)
            {
                txtMSV.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value?.ToString();
                txtTenSV.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value?.ToString();
                txtNganh.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value?.ToString();
                txtLop.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value?.ToString();
                txtDiem.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value?.ToString();
                txtMSV.Enabled = false;
            }

        }

        private void buttonXoa_Click(object sender, EventArgs e)

        {
            if (string.IsNullOrWhiteSpace(txtMSV.Text))
            {
                MessageBox.Show("Vui lòng chọn sinh viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable dt = Red($"SELECT * FROM QLSINHVIEN WHERE MASV = N'{txtMSV.Text}'");
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sinh viên để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Exe($"DELETE FROM QLSINHVIEN WHERE MASV=N'{txtMSV.Text}'");
                load();
                MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void buttonTK_Click(object sender, EventArgs e)
        {
            string searchValue = txtTK.Text;
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataTable dt = Red($"SELECT * FROM QLSINHVIEN WHERE MASV LIKE N'%{searchValue}%' OR TenSV LIKE N'%{searchValue}%'");
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sinh viên với thông tin tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                dataGridView1.DataSource = dt;
            }
        }
        //private void buttonResetTK_Click(object sender, EventArgs e)
        //{
        //    txtTK.Clear();
        //    load(); 
        //}

        private void txtTK_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Đăng xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            DangNhap loginForm = new DangNhap();       
            loginForm.Show();
            this.Hide();
        }

        private void txtMSV_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
