using GardenGroup.ViewModels;

namespace GardenGroup.StartupHelpers;

public interface IViewModelFactory
{
    T CreateViewModel<T>() where T : class;
    TViewModel CreateViewModel<TViewModel, TParameter>(TParameter mainViewModel) where TViewModel : class;
    public T CreateViewModelWithParameter<T, TParameter>(TParameter parameter) where T : class;
}