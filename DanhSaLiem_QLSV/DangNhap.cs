using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DanhSaLiem_QLSV
{
    public partial class DangNhap : Form
    {
        private SqlConnection connect = new SqlConnection("Server=LAPTOP-7KQ5GTV8;Database=QLSV;User Id=sa;Password=sa;");

        public DangNhap()
        {
            InitializeComponent();
            textBox_MKDN.PasswordChar = '*';
        }

        private void linkLabel_QuenMatKhau_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            QuenMatKhau quenMatKhau = new QuenMatKhau();
            quenMatKhau.ShowDialog();
        }

        private void linkLabel_DangKy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DangKy dangKy = new DangKy(); 
            dangKy.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (AuthenticateUser(textBox_TKDN.Text.Trim(), textBox_MKDN.Text.Trim()))
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Home home = new Home();
                home.Show();
                this.Hide(); 
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool AuthenticateUser(string taiKhoan, string matKhau)
        {
            bool isValid = false;

            using (SqlConnection connect = new SqlConnection("Server=LAPTOP-7KQ5GTV8;Database=QLSV;User Id=sa;Password=sa;"))
            {
                string query = "SELECT COUNT(*) FROM NguoiDung WHERE TaiKhoan = @TaiKhoan AND MatKhau = @MatKhau";
                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                    command.Parameters.AddWithValue("@MatKhau", matKhau);

                    try
                    {
                        connect.Open();
                        int count = (int)command.ExecuteScalar();
                        isValid = count > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            return isValid;
        }
   

private void DangNhap_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox_MKDN.PasswordChar = '\0';
            }
            else
            {
                textBox_MKDN.PasswordChar = '*';
            }
        }
    }
}
