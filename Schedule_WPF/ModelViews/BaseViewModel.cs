using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MainLibrary;
using Schedule_WPF.Common;
using Models;
using static MainLibrary.BookController;
using Schedule_WPF.Views;

namespace Schedule_WPF.ModelViews
{
    public class BaseViewModel: ViewModel
    {
        #region Members
        #region Menu
        private MenuItemType _currentMenuItemType = MenuItemType.MainMenu;
        public MenuItemType CurrentMenuItemType {
            get { return _currentMenuItemType; }
            set { _currentMenuItemType = value; this.SendPropertyChanged(nameof(CurrentMenuItemType)); }
        }
        #endregion

        #region Book
        private BookController _bookLayer;
        protected BookController BookLayer {
            get { return _bookLayer ?? (_bookLayer = new BookController()); }
        }

        public IEnumerable<TwoValues<string, int>> BookTypes {
            get {
                return new TwoValues<string, int>[] {
                    new TwoValues<string, int> { Item1 = "Учителя", Item2 = 0 },
                    new TwoValues<string, int> { Item1 = "Предметы", Item2 = 1 },
                    new TwoValues<string, int> { Item1 = "Аудитории", Item2 = 2 },
                    new TwoValues<string, int> { Item1 = "Группы", Item2 = 3 }
                };
            }
        }

        private int _selectedBookType = 0;
        public int SelectedBookType {
            get { return _selectedBookType; }
            set { _selectedBookType = value; this.SendPropertyChanged(nameof(SelectedBookType)); this.SendPropertyChanged(nameof(Books)); }
        }

        public IEnumerable<BookItem> Books {
            get {
                IEnumerable<BookItem> result;
                BookTypes type = (BookTypes)SelectedBookType;
                result = BookLayer.GetBookItems(type);
                return result;
            }
        }

        public IEnumerable<BookItem> Groups
        {
            get
            {
                IEnumerable<BookItem> result;
                result = BookLayer.GetBookItems(BookController.BookTypes.groups);
                return result;
            }
        }
        #endregion

        #region Schedule

        private ScheduleController _scheduleLayer;
        protected ScheduleController ScheduleLayer
        {
            get { return _scheduleLayer ?? (_scheduleLayer = new ScheduleController()); }
        }

        private BookItem _selectedGroup = new BookItem();
        public BookItem SelectedGroup
        {
            get { return _selectedGroup; }
            set { _selectedGroup = value;
                RefreshAllTables();
                this.SendPropertyChanged(nameof(SelectedGroup)); }
        }

        public IEnumerable<Schedule> ScheduleMonday
        {
            get
            {
                IEnumerable<Schedule> result;
                result = ScheduleLayer.GetScheduleForGroupByDay(SelectedGroup.Name,1);
                return result;
            }
        }
        public IEnumerable<Schedule> ScheduleTuesday
        {
            get
            {
                IEnumerable<Schedule> result;
                result = ScheduleLayer.GetScheduleForGroupByDay(SelectedGroup.Name, 2);
                return result;
            }
        }
        public IEnumerable<Schedule> ScheduleWednesday
        {
            get
            {
                IEnumerable<Schedule> result;
                result = ScheduleLayer.GetScheduleForGroupByDay(SelectedGroup.Name, 3);
                return result;
            }
        }
        public IEnumerable<Schedule> ScheduleThursday
        {
            get
            {
                IEnumerable<Schedule> result;
                result = ScheduleLayer.GetScheduleForGroupByDay(SelectedGroup.Name, 4);
                return result;
            }
        }
        public IEnumerable<Schedule> ScheduleFriday
        {
            get
            {
                IEnumerable<Schedule> result;
                result = ScheduleLayer.GetScheduleForGroupByDay(SelectedGroup.Name, 5);
                return result;
            }
        }
        public IEnumerable<Schedule> ScheduleSaturday
        {
            get
            {
                IEnumerable<Schedule> result;
                result = ScheduleLayer.GetScheduleForGroupByDay(SelectedGroup.Name, 6);
                return result;
            }
        }

        #endregion
        #endregion

        #region Commands
        #region Menu
        private Command _selectionPageCommand;
        public Command SelectionPageCommand {
            get {
                return _selectionPageCommand ?? (_selectionPageCommand = new Command(obj =>
                {
                    MenuItemType value = (MenuItemType)Convert.ToInt32(obj);
                    CurrentMenuItemType = value;
                }, obj => obj != null));
            }
        }
        #endregion
        #region Book
        private string _captionBook;
        public string CaptionBook {
            get { return _captionBook; }
            set { _captionBook = value; this.SendPropertyChanged(nameof(CaptionBook)); }
        }

        private Command _removeBookCommand;
        public Command RemoveBookCommand {
            get {
                return _removeBookCommand ?? (_removeBookCommand = new Command(obj =>
                {
                    BookItem value = (BookItem)obj;
                    BookLayer.DeleteBookItem(value.Id, (BookTypes)SelectedBookType);
                    this.SendPropertyChanged(nameof(Books));
                }, obj => obj != null));
            }
        }

        private Command _addBookCommand;
        public Command AddBookCommand {
            get {
                return _addBookCommand ?? (_addBookCommand = new Command(obj =>
                {
                    AddBookView addView = new AddBookView();
                    addView.DataContext = this;
                    if (addView.ShowDialog() == true) {
                        BookLayer.AddBookItem(new BookItem { Name = CaptionBook }, (BookTypes)SelectedBookType);
                        this.SendPropertyChanged(nameof(Books));
                    }
                }));
            }
        }

        private Command _closeWindowCommand;
        public Command CloseWindowCommand {
            get {
                return _closeWindowCommand ?? (_closeWindowCommand = new Command(obj =>
                {
                    Window win = (Window)obj;
                    win.DialogResult = true;
                    win.Close();
                }, obj => obj is Window));
            }
        }


        #endregion
        #region Schedule

        private int _unallocatedHours;
        public int UnallocatedHours
        {
            get { return _unallocatedHours; }
            set { _unallocatedHours = value; this.SendPropertyChanged(nameof(UnallocatedHours)); }
        }

        private Command _createSchedule;
        public Command CreateSchedule
        {
            get
            {
                return _createSchedule ?? (_createSchedule = new Command(obj =>
                {
                    ScheduleLayer.CreateScheduleForGroup(SelectedGroup.Name);
                    RefreshAllTables();
                }));
            }
        }

        private Command _enterKeyCommand;
        public Command EnterKeyCommand
        {
            get
            {
                return _enterKeyCommand ?? (_enterKeyCommand = new Command(obj =>
                {
                    RefreshAllTables();
                }));
            }
        }

        private Command _deleteKeyCommand;
        public Command DeleteKeyCommand
        {
            get
            {
                return _deleteKeyCommand ?? (_deleteKeyCommand = new Command(obj =>
                {
                    int value = (int)obj;
                    ScheduleLayer.DeleteById(value);
                    RefreshAllTables();
                }, obj => obj != null));
            }
        }

        private Command _checkSchedule;
        public Command CheckSchedule
        {
            get
            {
                return _checkSchedule ?? (_checkSchedule = new Command(obj =>
                {
                    var unsettings = ScheduleLayer.GetUnallocatedSubjects(SelectedGroup.Name);
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
                }));
            }
        }
        #endregion
        #endregion

        #region Helps
        private void RefreshAllTables()
        {
            this.SendPropertyChanged(nameof(ScheduleMonday));
            this.SendPropertyChanged(nameof(ScheduleTuesday));
            this.SendPropertyChanged(nameof(ScheduleWednesday));
            this.SendPropertyChanged(nameof(ScheduleThursday));
            this.SendPropertyChanged(nameof(ScheduleFriday));
            this.SendPropertyChanged(nameof(ScheduleSaturday));
            GetUnAllocatedHours();
            this.SendPropertyChanged(nameof(UnallocatedHours));
        }
        private void GetUnAllocatedHours()
        {
            int result = 0;
            var unsettings = ScheduleLayer.GetUnallocatedSubjects(SelectedGroup.Name);
            if (unsettings != null)
            {
                foreach (Settings set in unsettings)
                {
                    result += set.Hours;
                }
            }
            UnallocatedHours = result;
            this.SendPropertyChanged(nameof(UnallocatedHours));
        }
        #endregion
    }
}
