using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLSinhVienCT4.Model;

namespace QLSinhVienCT4
{
    public partial class Form1 : Form
    {

        // Tạo đối tượng đại diện database
        Model1 DbSinhVien = new Model1();
        public Form1()
        {
            InitializeComponent();
        }
        // tạo các phương thức đổ dữ liệu vào CBB, và DGV
        private void Form1_Load(object sender, EventArgs e)
        {
            // phương thức đổ dữ liệu vào bảng
            fillDGVSinhVien();
            fillCBBKhoa();
        }

        private void fillCBBKhoa()
        {
            // lấy ds khoa
            List<Faculty> listFaculty = DbSinhVien.Faculty.ToList();

            cbbKhoa.DataSource = listFaculty;

            cbbKhoa.DisplayMember = "FacultyName"; // tên hiển thị
            cbbKhoa.ValueMember = "FacultyID"; // giá trị xử lý
        }

        private void fillDGVSinhVien()
        {
            // lấy ds sinh viên
            List<Student> listStudent = DbSinhVien.Student.ToList();

            // đổ ds vào dgv
            foreach (Student student in listStudent)
            {
                // tạo dòng mới trong bảng
                int newRow = dgvSinhVien.Rows.Add();

                dgvSinhVien.Rows[newRow].Cells[0].Value = student.StudentID;
                dgvSinhVien.Rows[newRow].Cells[1].Value = student.FullName;
                dgvSinhVien.Rows[newRow].Cells[2].Value = student.AverageScore;
                dgvSinhVien.Rows[newRow].Cells[3].Value = student.Faculty.FacultyName;
            }
        }

        private void btnThemSua_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy thông tin từ các ô nhập liệu
                string studentID = txtMSSV.Text.Trim();
                string fullName = txtHoten.Text.Trim();
                double averageScore;
                if (!double.TryParse(txtDTB.Text, out averageScore))
                {
                    MessageBox.Show("Điểm trung bình không hợp lệ!");
                    return;
                }
                int facultyID = (int)cbbKhoa.SelectedValue;

                // Kiểm tra xem sinh viên có tồn tại hay không
                Student existingStudent = DbSinhVien.Student.Find(studentID);

                if (existingStudent != null)
                {
                    // Sửa thông tin sinh viên
                    existingStudent.FullName = fullName;
                    existingStudent.AverageScore = averageScore;
                    existingStudent.FacultyID = facultyID;

                    MessageBox.Show("Cập nhật thông tin sinh viên thành công!");
                }
                else
                {
                    // Thêm mới sinh viên
                    Student newStudent = new Student()
                    {
                        StudentID = studentID,
                        FullName = fullName,
                        AverageScore = averageScore,
                        FacultyID = facultyID
                    };

                    DbSinhVien.Student.Add(newStudent);
                    MessageBox.Show("Thêm sinh viên mới thành công!");
                }

                // Lưu thay đổi
                DbSinhVien.SaveChanges();

                // Cập nhật lại DGV
                dgvSinhVien.Rows.Clear();
                fillDGVSinhVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        private void Thoát_Click(object sender, EventArgs e)
        {
            
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy StudentID từ ô nhập liệu hoặc dòng đang chọn trong DataGridView
                if (dgvSinhVien.SelectedRows.Count > 0)
                {
                    // Lấy StudentID từ dòng đang chọn
                    string studentID = dgvSinhVien.SelectedRows[0].Cells[0].Value.ToString();

                    // Xác nhận trước khi xóa
                    DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        // Tìm sinh viên trong cơ sở dữ liệu
                        Student studentToDelete = DbSinhVien.Student.Find(studentID);
                        if (studentToDelete != null)
                        {
                            // Xóa sinh viên
                            DbSinhVien.Student.Remove(studentToDelete);
                            DbSinhVien.SaveChanges();

                            MessageBox.Show("Xóa sinh viên thành công!");

                            // Làm mới lại DataGridView
                            dgvSinhVien.Rows.Clear();
                            fillDGVSinhVien();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sinh viên để xóa!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một sinh viên để xóa!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        private void quảnLýKhoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }
    }
}
