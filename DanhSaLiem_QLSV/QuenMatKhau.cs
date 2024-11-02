using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DanhSaLiem_QLSV
{
    public partial class QuenMatKhau : Form
    {
        public QuenMatKhau()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string taiKhoan = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(taiKhoan))
            {
                MessageBox.Show("Vui lòng nhập tài khoản (email)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string matKhau = LayMatKhau(taiKhoan);
            if (matKhau != null)
            { 
                label3.Text = "Mật khẩu của bạn là: " + matKhau;
            }
            else
            {
                label3.Text = "Tài khoản không tồn tại!";
            }
        }

        private string LayMatKhau(string taiKhoan)
        {
            string matKhau = null;
            string connectionString = "Server=LAPTOP-7KQ5GTV8;Database=QLSV;User Id=sa;Password=sa;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT MatKhau FROM NguoiDung WHERE TaiKhoan = @TaiKhoan";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TaiKhoan", taiKhoan);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        matKhau = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return matKhau;
        }

        private void QuenMatKhau_Load(object sender, EventArgs e)
        {

        }
    }
}
