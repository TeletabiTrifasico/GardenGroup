using GardenGroup.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Service;

namespace GardenGroup.StartupHelpers;

public class ViewModelFactory(IServiceProvider serviceProvider) : IViewModelFactory
{
    public T CreateViewModel<T>() where T : class
    {
        return serviceProvider.GetRequiredService<T>();
    }
    
    // Use reflection to create an instance, passing the parameter
    public T CreateViewModel<T, TParameter>(TParameter parameter) where T : class
    {
        var serviceManager = serviceProvider.GetRequiredService<IServiceManager>();
        var constructorInfo = typeof(T).GetConstructor(new[] { typeof(IServiceManager), typeof(MainViewModel) });
        
        if (constructorInfo == null)
            throw new MissingMethodException($"Constructor on type '{typeof(T).Name}' not found.");
        
        return (T)constructorInfo.Invoke(new object[] { serviceManager, parameter });
    }
}