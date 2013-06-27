using System.ComponentModel;

namespace EggFarmSystem.Client.Core
{
    public interface IPagingInfo: INotifyPropertyChanged
    {
        int PageSize { get; set; }

        int PageIndex { get; set; }

        int TotalPage { get; set; }
    }
}
