using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {
        private int _studentId;
        private Student _student;
        private List<Group> _groups;
        private FileHelper<List<Student>> _fileHelper =
           new FileHelper<List<Student>>(Program.FilePath);
        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            _studentId = id;
            _groups = GroupsHelper.GetGroup("Brak");
            InitGroupsCombobox();
            GetStudentData();
            tbFirstName.Select();
        }

        private void InitGroupsCombobox()
        {
            cmbGroups.DataSource = _groups;
            cmbGroups.DisplayMember = "Name";
            cmbGroups.ValueMember = "Id";
        }

       
        private void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edycja danych ucznia";

                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(s => s.Id == _studentId);
                if (_student == null)
                {
                    throw new Exception("Uczeń o podanym Id nie istnieje.");
                }

                FillTextBoxes();

            }
        }

        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            tbTechnology.Text = _student.Technology;
            tbMath.Text = _student.Math;
            tbPhysics.Text = _student.Physics;
            tbPolishLanguage.Text = _student.PolishLang;
            tbForeignLanguage.Text = _student.ForeignLang;
            rtbComments.Text = _student.Comments;
            cbAttendingAdditionalClasses.Checked = _student.AttendingAdditionalClasses;
            cmbGroups.SelectedItem = _groups.FirstOrDefault(x => x.Id == _student.GroupId);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
            {
                students.RemoveAll(s => s.Id == _studentId);
            }
            else

                AssignIdToNewStudent(students);
            AddNewStudentToList(students);

            _fileHelper.SerializeToFile(students);

            Close();
        }

        private void AssignIdToNewStudent(List<Student> students)
        {
            var studentWithHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();
            _studentId = studentWithHighestId == null ? 1 : studentWithHighestId.Id + 1;
        }

        private void AddNewStudentToList(List<Student> students)
        {
            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = rtbComments.Text,
                Math = tbMath.Text,
                Physics = tbPhysics.Text,
                Technology = tbTechnology.Text,
                PolishLang = tbPolishLanguage.Text,
                ForeignLang = tbForeignLanguage.Text,
                AttendingAdditionalClasses = cbAttendingAdditionalClasses.Checked,
                GroupId = (cmbGroups.SelectedItem as Group).Id,
                GroupName = (cmbGroups.SelectedItem as Group).Name,
            };

            if (student.FirstName == string.Empty || student.LastName == string.Empty)
            {
                MessageBox.Show("Nie podano Imienia i/lub nazwiska ucznia. Kliknij OK po czym uzupełnij dane.");
            }

            students.Add(student);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
