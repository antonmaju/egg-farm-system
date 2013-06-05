using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace EggFarmSystem.Client.Core.Views
{
    public interface IMainView
    {
        void Initialize();

        void ChangeView(UserControl newView);
    }
}
