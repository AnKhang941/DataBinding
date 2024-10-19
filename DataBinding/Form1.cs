using DataBinding.Entities;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DataBinding
{
    public partial class Form1 : Form
    {
        SchoolDB context = new SchoolDB();
        private BindingSource bindingSource = new BindingSource();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            BindControls();
        }

        private void LoadData()
        {
            bindingSource.DataSource = context.Students.ToList();
            dgvDSSV.DataSource = bindingSource;
            bindingNavigator1.BindingSource = bindingSource;

            if (bindingSource.Count > 0)
            {
                bindingSource.Position = 0;
            }
        }

        private void BindControls()
        {
            txtTen.DataBindings.Clear();
            txtAge.DataBindings.Clear();
            cmbMajor.DataBindings.Clear();

            txtTen.DataBindings.Add("Text", bindingSource, "FullName", true, DataSourceUpdateMode.OnPropertyChanged);
            txtAge.DataBindings.Add("Text", bindingSource, "Age", true, DataSourceUpdateMode.OnPropertyChanged);
            cmbMajor.DataBindings.Add("Text", bindingSource, "Major", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    Student newStudent = new Student
                    {
                        FullName = txtTen.Text,
                        Age = int.Parse(txtAge.Text),
                        Major = cmbMajor.Text
                    };
                    context.Students.Add(newStudent);
                    context.SaveChanges();
                    LoadData();
                    MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (bindingSource.Current is Student student && ValidateInput())
                {
                    student.FullName = txtTen.Text;
                    student.Age = int.Parse(txtAge.Text);
                    student.Major = cmbMajor.Text;

                    context.SaveChanges();
                    LoadData();
                    MessageBox.Show("Sửa thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa thông tin sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (bindingSource.Current is Student student)
                {
                    var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này không?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        context.Students.Remove(student);
                        context.SaveChanges();
                        LoadData();
                        MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!int.TryParse(txtAge.Text, out int age) || age <= 0)
            {
                MessageBox.Show("Vui lòng nhập tuổi hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbMajor.Text))
            {
                MessageBox.Show("Vui lòng chọn ngành học!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn đóng ứng dụng không?", "Xác nhận đóng", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
