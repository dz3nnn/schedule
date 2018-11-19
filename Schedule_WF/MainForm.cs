using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MainLibrary;
using Models;
using System.Linq;

namespace Schedule_WF
{
    public partial class MainForm : Form
    {

        readonly BookController bk = new BookController();
        readonly SettingsController sc = new SettingsController();
        readonly ScheduleController schc = new ScheduleController();
        readonly Helper helper = new Helper();

        List<DataGridView> dgList = new List<DataGridView>();

        public MainForm()
        {
            InitializeComponent();
            dgList.Add(dgSchedule1);
            dgList.Add(dgSchedule2);
            dgList.Add(dgSchedule3);
            dgList.Add(dgSchedule4);
            dgList.Add(dgSchedule5);
            dgList.Add(dgSchedule6);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            helper.FillData(dgBook, (BookController.BookTypes)cbBookTypes.SelectedIndex);
        }

        private void cbGroups_Enter(object sender, EventArgs e)
        {
            helper.FillComboByBookItems(cbGroups,BookController.BookTypes.groups);
        }

        private void cbSubjects_Enter(object sender, EventArgs e)
        {
            helper.FillComboByBookItems(cbSubjects,BookController.BookTypes.subjects);
        }

        private void cbTeachers1_Enter(object sender, EventArgs e)
        {
            helper.FillComboByBookItems(cbTeachers1,BookController.BookTypes.teachers);
        }

        private void cbTeachers2_Enter(object sender, EventArgs e)
        {
            helper.FillComboByBookItems(cbTeachers2, BookController.BookTypes.teachers);
        }

        private void cbRooms1_Enter(object sender, EventArgs e)
        {
            helper.FillComboByBookItems(cbRooms1, BookController.BookTypes.rooms);
        }

        private void cbGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgSettings.Rows.Clear();
            var data = sc.GetSettingsForGroup(cbGroups.SelectedItem.ToString());
            if (data.Count() != 0)
                foreach (Settings set in data)
                    dgSettings.Rows.Add(set.Id,set.Subject,set.Teacher1,set.Teacher2,set.Room,set.Hours,set.HoursAll,set.HoursDay);
        }

        private void btnAddSettings_Click(object sender, EventArgs e)
        {
            sc.AddSettings(new Settings {
                GroupName = cbGroups.SelectedItem.ToString(),
                Hours = int.Parse(tbHours.Text),
                Room = cbRooms1.SelectedItem.ToString(),
                Subject = cbSubjects.SelectedItem.ToString(),
                Teacher1 = cbTeachers1.SelectedItem.ToString(),
                Teacher2 = cbTeachers2.SelectedItem.ToString(),
                HoursAll = int.Parse(tbHoursAll.Text),
                HoursDay = int.Parse(tbHoursDay.Text)
            });
            cbGroups_SelectedIndexChanged(sender,e);
        }

        private void dgSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                if (dgSettings.SelectedRows[0].Index >= 0)
                {
                    if (helper.OnKeyDelete())
                    {
                        sc.DeleteSettings(int.Parse(dgSettings.SelectedRows[0].Cells[0].Value.ToString()));
                        dgSettings.Rows.Remove(dgSettings.SelectedRows[0]);
                    }
                }
            }
        }

        private void dgBook_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                if (dgBook.SelectedRows[0].Index >= 0)
                {
                    if (helper.OnKeyDelete())
                    {
                        bk.DeleteBookItem(int.Parse(dgBook.SelectedRows[0].Cells[0].Value.ToString()), (BookController.BookTypes)cbBookTypes.SelectedIndex);
                        dgBook.Rows.Remove(dgBook.SelectedRows[0]);
                    }
                }
                lbGroups_Enter(sender, e);
            }
            if(e.KeyCode == Keys.Enter)
            {
                AddBookItem abi = new AddBookItem(((BookController.BookTypes)cbBookTypes.SelectedIndex).ToString());
                abi.ShowDialog();
                comboBox1_SelectedIndexChanged(sender,e);
                lbGroups_Enter(sender, e);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cbBookTypes.SelectedIndex = 0;
            cbMonth.SelectedIndex = DateTime.Now.Month-1;
            helper.FillEmptySchedule(dgList);
            lbGroups_Enter(sender, e);
            helper.LoadGroupsForReading(listReading);
            listReading.ContextMenuStrip = contextMenuStrip1;
        }

        private void btnCreateSchedule_Click(object sender, EventArgs e)
        {
            var check = schc.GetScheduleForGroup(lbGroups.SelectedItem.ToString());
            schc.CreateScheduleForGroup(lbGroups.SelectedItem.ToString());
            lbGroups_SelectedIndexChanged(sender, e);
        }

        private void btnAcceptSchedule_Click(object sender, EventArgs e)
        {
            var unsettings = schc.GetUnallocatedSubjects(lbGroups.SelectedItem.ToString());
            if (unsettings != null)
            {
                string show = null;
                foreach (Settings set in unsettings)
                {
                    show += string.Format("{0}({1})\n", set.Subject, set.Hours);
                }
                MessageBox.Show(show);
            }
            else
                MessageBox.Show("Все сходится.");
        }

        private void dgSchedule1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                if (helper.OnKeyDelete())
                {
                    schc.DeleteById(int.Parse(dgSchedule1[0, dgSchedule1.SelectedRows[0].Index].Value.ToString()));
                    lbGroups_SelectedIndexChanged(sender,e);
                }
            }
            if (e.KeyCode == Keys.Enter)
                this.dgSchedule1_CellDoubleClick(new object(),new DataGridViewCellEventArgs(0,0));
        }

        private void dgSchedule1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgSchedule1[1, dgSchedule1.SelectedRows[0].Index].Value == null)
            {
                AddScheduleForm addScheduleForm = new AddScheduleForm(new Schedule { WeekDay = 1, GroupName = lbGroups.SelectedItem.ToString(), Lesson = dgSchedule1.SelectedRows[0].Index + 1 });
                addScheduleForm.ShowDialog();
                lbGroups_SelectedIndexChanged(sender, e);
            }
        }

        private void lbGroups_Enter(object sender, EventArgs e)
        {
            lbGroups.Items.Clear();
            var data = bk.GetBookItems(BookController.BookTypes.groups);
            if (data.Count() != 0)
                foreach (BookItem bi in data)
                    lbGroups.Items.Add(bi.Name);
        }

        private void lbGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            helper.ClearSchedule(dgList);
            helper.FillEmptySchedule(dgList);
            if(lbGroups.SelectedIndex >= 0)
            {
                var schedule = schc.GetScheduleForGroup(lbGroups.SelectedItem.ToString());
                if (schedule.Count() != 0)
                {
                    foreach (Schedule sc in schedule)
                        helper.InputSchedule(sc,dgList);
                }
            }
        }

        private void dgSchedule2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgSchedule2[1, dgSchedule2.SelectedRows[0].Index].Value == null)
            {
                AddScheduleForm addScheduleForm = new AddScheduleForm(new Schedule { WeekDay = 2, GroupName = lbGroups.SelectedItem.ToString(), Lesson = dgSchedule2.SelectedRows[0].Index + 1 });
                addScheduleForm.ShowDialog();
                lbGroups_SelectedIndexChanged(sender, e);
            }
        }

        private void dgSchedule2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
               if (helper.OnKeyDelete())
                {
                    schc.DeleteById(int.Parse(dgSchedule2[0, dgSchedule2.SelectedRows[0].Index].Value.ToString()));
                    lbGroups_SelectedIndexChanged(sender, e);
                }
            }
            if (e.KeyCode == Keys.Enter)
                this.dgSchedule2_CellDoubleClick(new object(), new DataGridViewCellEventArgs(0, 0));
        }

        private void dgSchedule3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgSchedule3[1, dgSchedule3.SelectedRows[0].Index].Value == null)
            {
                AddScheduleForm addScheduleForm = new AddScheduleForm(new Schedule { WeekDay = 3, GroupName = lbGroups.SelectedItem.ToString(), Lesson = dgSchedule3.SelectedRows[0].Index + 1 });
                addScheduleForm.ShowDialog();
                lbGroups_SelectedIndexChanged(sender, e);
            }
        }

        private void dgSchedule3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (helper.OnKeyDelete())
                {
                    schc.DeleteById(int.Parse(dgSchedule3[0, dgSchedule3.SelectedRows[0].Index].Value.ToString()));
                    lbGroups_SelectedIndexChanged(sender, e);
                }
            }
            if (e.KeyCode == Keys.Enter)
                this.dgSchedule3_CellDoubleClick(new object(), new DataGridViewCellEventArgs(0, 0));
        }

        private void dgSchedule4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgSchedule4[1, dgSchedule4.SelectedRows[0].Index].Value == null)
            {
                AddScheduleForm addScheduleForm = new AddScheduleForm(new Schedule { WeekDay = 4, GroupName = lbGroups.SelectedItem.ToString(), Lesson = dgSchedule4.SelectedRows[0].Index + 1 });
                addScheduleForm.ShowDialog();
                lbGroups_SelectedIndexChanged(sender, e);
            }
        }

        private void dgSchedule4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
               if (helper.OnKeyDelete())
                {
                    schc.DeleteById(int.Parse(dgSchedule4[0, dgSchedule4.SelectedRows[0].Index].Value.ToString()));
                    lbGroups_SelectedIndexChanged(sender, e);
                }
            }
            if (e.KeyCode == Keys.Enter)
                this.dgSchedule4_CellDoubleClick(new object(), new DataGridViewCellEventArgs(0, 0));
        }

        private void dgSchedule5_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgSchedule5[1, dgSchedule5.SelectedRows[0].Index].Value == null)
            {
                AddScheduleForm addScheduleForm = new AddScheduleForm(new Schedule { WeekDay = 5, GroupName = lbGroups.SelectedItem.ToString(), Lesson = dgSchedule5.SelectedRows[0].Index + 1 });
                addScheduleForm.ShowDialog();
                lbGroups_SelectedIndexChanged(sender, e);
            }
        }

        private void dgSchedule5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (helper.OnKeyDelete())
                {
                    schc.DeleteById(int.Parse(dgSchedule5[0, dgSchedule5.SelectedRows[0].Index].Value.ToString()));
                    lbGroups_SelectedIndexChanged(sender, e);
                }
            }
            if (e.KeyCode == Keys.Enter)
                this.dgSchedule5_CellDoubleClick(new object(), new DataGridViewCellEventArgs(0, 0));
        }

        private void dgSchedule6_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgSchedule6[1, dgSchedule6.SelectedRows[0].Index].Value == null)
            {
                AddScheduleForm addScheduleForm = new AddScheduleForm(new Schedule { WeekDay = 6, GroupName = lbGroups.SelectedItem.ToString(), Lesson = dgSchedule6.SelectedRows[0].Index + 1 });
                addScheduleForm.ShowDialog();
                lbGroups_SelectedIndexChanged(sender, e);
            }
        }

        private void dgSchedule6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
               if (helper.OnKeyDelete())
                {
                    schc.DeleteById(int.Parse(dgSchedule6[0, dgSchedule6.SelectedRows[0].Index].Value.ToString()));
                    lbGroups_SelectedIndexChanged(sender, e);
                }
            }
            if (e.KeyCode == Keys.Enter)
                this.dgSchedule6_CellDoubleClick(new object(), new DataGridViewCellEventArgs(0, 0));
        }

        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && listReading.SelectedIndex >=0)
            {
                listReading.SetItemChecked(listReading.SelectedIndex,!listReading.GetItemChecked(listReading.SelectedIndex));
            }
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
            helper.PrintReading();
        }
    }
}
