using System.Windows.Input;

namespace BlackList.Ui.Wpf.Host.ViewModels
{
    public class ConfirmationViewModel
    {
        public ConfirmationViewModel() 
        {            
        }

        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }
    }
}
