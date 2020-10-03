using SMGApp.WPF.States.Navigators;

namespace SMGApp.WPF.ViewModels.Factories
{
    public interface IRootSMGAppViewModelAbstractFactory
    {
        ViewModelBase CreateViewModel(ViewType viewType);
    }
}
