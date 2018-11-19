using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using MainLibrary;
using System.Windows.Forms;

namespace Schedule_WF
{
    /// <summary>
    /// Класс содержащий вспомогающие функции
    /// </summary>
    public class Helper
    {
        readonly SettingsController settingsController = new SettingsController();
        readonly ScheduleController scheduleController = new ScheduleController();
        readonly BookController bookController = new BookController();

        /// <summary>
        /// Наполнение ComboBox на основании типа справочника
        /// </summary>
        /// <param name="cb">ComboBox</param>
        /// <param name="bk">Тип справочника</param>
        public void FillComboByBookItems(ComboBox cb,BookController.BookTypes bk) 
        {
            cb.Items.Clear();
            var data = bookController.GetBookItems(bk);
            if (data.Count() != 0)
                foreach (BookItem bi in data)
                    cb.Items.Add(bi.Name);
        }

        /// <summary>
        /// Диалоговое окно с подтверждением удаления
        /// </summary>
        /// <returns></returns>
        public bool OnKeyDelete()
        {
            var result = MessageBox.Show("Подтвердите удаление", "Подтверждение", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                return true;
            return false;
        }

        /// <summary>
        /// Заполнение таблиц расписания пустыми строками
        /// </summary>
        /// <param name="dgList">Список таблиц</param>
        public void FillEmptySchedule(List<DataGridView> dgList)
        {
            for (int i = 1; i <= 11; i++)
            {
                foreach (DataGridView dg in dgList)
                    dg.Rows.Add();
            }
        }

        /// <summary>
        /// Заполнение таблиц расписания расписанием
        /// </summary>
        /// <param name="sch">Расписание</param>
        /// <param name="dgList">Список таблиц</param>
        public void InputSchedule(Schedule sch, List<DataGridView> dgList)
        {
            for (int i = 0; i < dgList.ToArray().Length; i++)
            {
                if (i + 1 == sch.WeekDay)
                {
                    dgList[i][0, sch.Lesson - 1].Value = sch.Id;
                    dgList[i][1, sch.Lesson - 1].Value = sch.Subject;
                    dgList[i][2, sch.Lesson - 1].Value = sch.Room;
                }
            }
        }

        /// <summary>
        /// Очищение таблиц расписания
        /// </summary>
        /// <param name="dgList">Список таблиц</param>
        public void ClearSchedule(List<DataGridView> dgList)
        {
            foreach (DataGridView dg in dgList)
                dg.Rows.Clear();
        }

        /// <summary>
        /// Печать занятости
        /// </summary>
        public void PrintReading()
        {
            MessageBox.Show("Start Printing");
        }

        /// <summary>
        /// Заполнение таблицы из справочника
        /// </summary>
        /// <param name="dg">Таблица</param>
        /// <param name="btypes">Тип справочника</param>
        public void FillData(DataGridView dg, BookController.BookTypes btypes)
        {
            dg.Rows.Clear();
            var data = bookController.GetBookItems(btypes);
            if (data.Count() != 0)
                foreach (BookItem bi in data)
                    dg.Rows.Add(bi.Id, bi.Name);
        }

        /// <summary>
        /// Вычитка
        /// </summary>
        public void LoadGroupsForReading(CheckedListBox listReading)
        {
            listReading.Items.Clear();
            var groups = bookController.GetBookItems(BookController.BookTypes.groups);
            foreach (var bi in groups)
                listReading.Items.Add(bi.Name);
        }
    }
}
