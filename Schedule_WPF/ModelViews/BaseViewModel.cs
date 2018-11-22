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
                //result = BookLayer.GetBookItems(type);
                result = new BookItem[] {
                    new BookItem { Id = 1, Name = "per" }
                };
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
                        //BookLayer.AddBookItem(new BookItem { Name = CaptionBook }, ((BookTypes)SelectedBookType).ToString());
                        MessageBox.Show(CaptionBook);
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
        #endregion

        #region Helps

        #endregion
    }
}
