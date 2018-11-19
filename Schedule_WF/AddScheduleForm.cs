using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainLibrary;
using Models;

namespace Schedule_WF
{
    public partial class AddScheduleForm : Form
    {
        Schedule mainSchedule = new Schedule();
        public AddScheduleForm()
        {
            InitializeComponent();
        }

        public AddScheduleForm(Schedule schedule)
        {
            InitializeComponent();
            mainSchedule = schedule;
            ScheduleController scheduleController = new ScheduleController();
            var unloc = scheduleController.GetUnallocatedSubjects(schedule.GroupName);
            if (unloc != null)
            {
                foreach (Settings set in unloc)
                {
                    if (scheduleController.CanInputHere(new Schedule { Lesson = schedule.Lesson, WeekDay = schedule.WeekDay, Room = set.Room, Teacher1 = set.Teacher1, GroupName = set.GroupName }))
                    {
                        dataGridView1.Rows.Add(set.Subject, set.Hours);
                    }
                }
            }
            dataGridView1.Rows.Add("Обед");
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ScheduleController scheduleController = new ScheduleController();
                var unloc = scheduleController.GetUnallocatedSubjects(mainSchedule.GroupName);
                foreach(Settings set in unloc)
                {
                    if(set.Subject == dataGridView1[0, dataGridView1.SelectedRows[0].Index].Value.ToString())
                    {
                        scheduleController.InputSchedule(new Schedule
                        {
                            GroupName = set.GroupName,
                            Lesson = mainSchedule.Lesson,
                            Room = set.Room,
                            Subject = set.Subject,
                            Teacher1 = set.Teacher1,
                            Teacher2 = set.Teacher2,
                            WeekDay = mainSchedule.WeekDay
                        });
                    }
                }
                if (dataGridView1[0, dataGridView1.SelectedRows[0].Index].Value.ToString() == "Обед")
                    scheduleController.InputSchedule(new Schedule {
                        GroupName = mainSchedule.GroupName,
                        Subject = "Обед",
                        Lesson = mainSchedule.Lesson,
                        WeekDay = mainSchedule.WeekDay
                    });
                this.Close();
            }
        }

        private void AddScheduleForm_Load(object sender, EventArgs e)
        {

        }
    }
}
