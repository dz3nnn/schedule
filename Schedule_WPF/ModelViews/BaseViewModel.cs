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

        private string _captionBook;
        public string CaptionBook
        {
            get { return _captionBook; }
            set { _captionBook = value; this.SendPropertyChanged(nameof(CaptionBook)); }
        }


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

        public IEnumerable<BookItem> Teachers
        {
            get
            {
                IEnumerable<BookItem> result;
                result = BookLayer.GetBookItems(BookController.BookTypes.teachers);
                return result;
            }
        }

        public IEnumerable<BookItem> Rooms
        {
            get
            {
                IEnumerable<BookItem> result;
                result = BookLayer.GetBookItems(BookController.BookTypes.rooms);
                return result;
            }
        }

        public IEnumerable<BookItem> Subjects
        {
            get
            {
                IEnumerable<BookItem> result;
                result = BookLayer.GetBookItems(BookController.BookTypes.subjects);
                return result;
            }
        }
        #endregion

        #region Schedule

        private IEnumerable<Settings> _unallocatedSchedule;
        public IEnumerable<Settings> UnallocatedSchedule
        {
            get { return _unallocatedSchedule; }
            set { _unallocatedSchedule = value; this.SendPropertyChanged(nameof(UnallocatedSchedule)); }
        }

        private Schedule _addingSchedule;
        public Schedule AddingSchedule
        {
            get { return _addingSchedule ?? (_addingSchedule = new Schedule()); }
            set { _addingSchedule = value; this.SendPropertyChanged(nameof(AddingSchedule)); }
        }

        private int _unallocatedHours;
        public int UnallocatedHours
        {
            get { return _unallocatedHours; }
            set { _unallocatedHours = value; this.SendPropertyChanged(nameof(UnallocatedHours)); }
        }

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
                //System.Threading.Thread.Sleep(5000); // если хочешь посмотреть как работает анимация
                IEnumerable<Schedule> result;
                result = ScheduleLayer.GetScheduleForGroupByDay(SelectedGroup.Name,1);
                result = FillEmpty(result,1);
                return result;
            }
        }
        public IEnumerable<Schedule> ScheduleTuesday
        {
            get
            {
                IEnumerable<Schedule> result;
                result = ScheduleLayer.GetScheduleForGroupByDay(SelectedGroup.Name, 2);
                result = FillEmpty(result,2);
                return result;
            }
        }
        public IEnumerable<Schedule> ScheduleWednesday
        {
            get
            {
                IEnumerable<Schedule> result;
                result = ScheduleLayer.GetScheduleForGroupByDay(SelectedGroup.Name, 3);
                result = FillEmpty(result,3);
                return result;
            }
        }
        public IEnumerable<Schedule> ScheduleThursday
        {
            get
            {
                IEnumerable<Schedule> result;
                result = ScheduleLayer.GetScheduleForGroupByDay(SelectedGroup.Name, 4);
                result = FillEmpty(result,4);
                return result;
            }
        }
        public IEnumerable<Schedule> ScheduleFriday
        {
            get
            {
                IEnumerable<Schedule> result;
                result = ScheduleLayer.GetScheduleForGroupByDay(SelectedGroup.Name, 5);
                result = FillEmpty(result,5);
                return result;
            }
        }
        public IEnumerable<Schedule> ScheduleSaturday
        {
            get
            {
                IEnumerable<Schedule> result;
                result = ScheduleLayer.GetScheduleForGroupByDay(SelectedGroup.Name, 6);
                result = FillEmpty(result,6);
                return result;
            }
        }

        

        #endregion

        #region Settings

        private SettingsController _settingsLayer;
        protected SettingsController SettingsLayer
        {
            get { return _settingsLayer ?? (_settingsLayer = new SettingsController()); }
        }

        public IEnumerable<Settings> AllSettings
        {
            get
            {
                IEnumerable<Settings> result;
                result = SettingsLayer.GetSettingsForGroup(SelectedGroup.Name);
                return result;
            }
        }

        private Settings _addingSettings;
        public Settings AddingSettings
        {
            get { return _addingSettings ?? (_addingSettings = new Settings()); }
            set
            {
                _addingSettings = value;
                this.SendPropertyChanged(nameof(AddingSettings));
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
                    Schedule schedule = (Schedule)obj;
                    AddingSchedule = schedule;
                    AddScheduleView addView = new AddScheduleView();
                    addView.DataContext = this;
                    GetUnAllocatedSchedule(schedule);
                    if (addView.ShowDialog() == true)
                    {
                        RefreshAllTables();
                    }
                }, obj => obj != null));
            }
        }

        private Command _enterAddingKeyCommand;
        public Command EnterAddingKeyCommand
        {
            get
            {
                return _enterAddingKeyCommand ?? (_enterAddingKeyCommand = new Command(obj =>
                {
                Settings sett = AddingSettings;
                foreach (Settings set in UnallocatedSchedule)
                {
                    if (set.Subject == sett.Subject)
                    {
                        ScheduleLayer.InputSchedule(new Schedule
                        {
                            GroupName = set.GroupName,
                            Lesson = AddingSchedule.Lesson,
                            Room = set.Room,
                            Subject = set.Subject,
                            Teacher1 = set.Teacher1,
                            Teacher2 = set.Teacher2,
                            WeekDay = AddingSchedule.WeekDay
                        });
                    }
                }
                if (sett.Subject == "Обед")
                    ScheduleLayer.InputSchedule(new Schedule
                    {
                        GroupName = AddingSchedule.GroupName,
                        Subject = "Обед",
                        Lesson = AddingSchedule.Lesson,
                        WeekDay = AddingSchedule.WeekDay
                    });
                RefreshAllTables();
                    CloseWindowCommand.Execute(obj);
                }, obj => obj != null));
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

        #region Settings

        private Command _addSettingsCommand;
        public Command AddSettingsCommand
        {
            get
            {
                return _addSettingsCommand ?? (_addSettingsCommand = new Command(obj =>
                {
                    _addingSettings = new Settings();
                    AddSettingsView addView = new AddSettingsView();
                    addView.DataContext = this;
                    if (addView.ShowDialog() == true)
                    {
                        SettingsLayer.AddSettings(AddingSettings);
                        this.SendPropertyChanged(nameof(AllSettings));
                    }
                }));
            }
        }

        private Command _addSettingsViewCommand;
        public Command AddSettingsViewCommand
        {
            get
            {
                return _addSettingsViewCommand ?? (_addSettingsViewCommand = new Command(obj =>
                {
                    SettingsLayer.AddSettings(AddingSettings);
                }));
            }
        }

        private Command _deleteSettingsKeyCommand;
        public Command DeleteSettingsKeyCommand
        {
            get
            {
                return _deleteSettingsKeyCommand ?? (_deleteSettingsKeyCommand = new Command(obj =>
                {
                    int setid = (int)obj;
                    SettingsLayer.DeleteSettings(setid);
                    RefreshAllTables();
                }, obj => obj != null));
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
            this.SendPropertyChanged(nameof(AllSettings));
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
        private IEnumerable<Schedule> FillEmpty(IEnumerable<Schedule> schedules, int day)
        {
            List<Schedule> newresult = new List<Schedule>();
            for (int i = 1; i <= 11; i++)
            {
                bool find = false;
                foreach (Schedule schedule in schedules)
                {
                    if (schedule.Lesson == i)
                    {
                        find = true;
                        newresult.Add(schedule);
                    }
                }
                if (!find)
                    newresult.Add(new Schedule {Lesson = i, WeekDay =day ,GroupName = SelectedGroup.Name});
            }
            return newresult;
        }
        private void GetUnAllocatedSchedule(Schedule schedule)
        {
            var unloc = ScheduleLayer.GetUnallocatedSubjects(schedule.GroupName);
            List<Settings> result = new List<Settings>();
            if (unloc != null)
            {
                foreach (Settings set in unloc)
                {
                    if (ScheduleLayer.CanInputHere(new Schedule { Lesson = schedule.Lesson, WeekDay = schedule.WeekDay, Room = set.Room, Teacher1 = set.Teacher1, GroupName = set.GroupName }))
                    {
                        result.Add(set);
                    }
                }
            }
            result.Add(new Settings { Subject = "Обед" });
            UnallocatedSchedule = result;
            this.SendPropertyChanged(nameof(UnallocatedSchedule));
        }
        #endregion
    }
}
