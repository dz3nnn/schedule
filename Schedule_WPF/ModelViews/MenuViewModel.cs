using Schedule_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schedule_WPF.ModelViews
{
    public class MenuViewModel: ViewModel
    {
        #region Memebrs
        private MenuItemType _currentMenuItemType = MenuItemType.MainMenu;
        public MenuItemType CurrentMenuItemType {
            get { return _currentMenuItemType; }
            set { _currentMenuItemType = value; this.SendPropertyChanged(nameof(CurrentMenuItemType)); }
        }

        #endregion

        #region Commands

        #endregion

        #region Helps

        #endregion
    }
}
