using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BlackList.Ui.Wpf.Host.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        public void OnPropertyChanged(object sender, [CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(caller));
        }

    }
}
