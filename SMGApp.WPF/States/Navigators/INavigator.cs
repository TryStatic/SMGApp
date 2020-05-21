using System.Windows.Input;
using SMGApp.WPF.ViewModels;

namespace SMGApp.WPF.States.Navigators
{
    public enum ViewType {
        Customer,
        Service,
        Inventory,
        Backup,
        Guarantee
    }

    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        ICommand UpdateCurrentViewModelCommand { get; }
    }
}
