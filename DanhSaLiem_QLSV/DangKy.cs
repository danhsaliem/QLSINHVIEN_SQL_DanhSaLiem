using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DanhSaLiem_QLSV
{
    public partial class DangKy : Form
    {

        SqlConnection connect = new SqlConnection("Server=LAPTOP-7KQ5GTV8;Database=QLSV;User Id=sa;Password=sa;");

        public DangKy()
        {
            InitializeComponent();
            textBox_MK.PasswordChar = '*';
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(textBox_TK.Text) || !textBox_TK.Text.Contains("@gmail.com"))
            {
                MessageBox.Show("Tài khoản phải chứa  '@gmail.com'!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox_MK.Text))
            {
                MessageBox.Show("Mật khẩu không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (textBox_MK.Text.Length < 8)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return; 
            }
            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();
                    string checkUserQuery = "SELECT * FROM NguoiDung WHERE TaiKhoan = @TaiKhoan";
                    using (SqlCommand checkUser = new SqlCommand(checkUserQuery, connect))
                    {
                        checkUser.Parameters.AddWithValue("@TaiKhoan", textBox_TK.Text.Trim());

                        SqlDataAdapter adapter = new SqlDataAdapter(checkUser);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        if (table.Rows.Count >= 1)
                        {
                            MessageBox.Show(textBox_TK.Text + " đã tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            string insertData = "INSERT INTO NguoiDung(TaiKhoan, MatKhau) VALUES(@TaiKhoan, @MatKhau)";
                            using (SqlCommand cmd = new SqlCommand(insertData, connect))
                            {
                                cmd.Parameters.AddWithValue("@TaiKhoan", textBox_TK.Text.Trim());
                                cmd.Parameters.AddWithValue("@MatKhau", textBox_MK.Text.Trim());

                                cmd.ExecuteNonQuery();

                                MessageBox.Show("Đăng Ký Thành Công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                DangNhap dangNhap = new DangNhap();
                                dangNhap.Show();
                                this.Hide();

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        private void DangKy_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
