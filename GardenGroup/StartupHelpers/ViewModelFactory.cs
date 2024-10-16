using GardenGroup.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Service;

namespace GardenGroup.StartupHelpers;

/// <summary>
/// Abstract factory pattern
/// </summary>
/// <param name="serviceProvider"></param>
public class ViewModelFactory(IServiceProvider serviceProvider) : IViewModelFactory
{
    public T CreateViewModel<T>() where T : class
    {
        return serviceProvider.GetRequiredService<T>();
    }
    
    /// <summary>
    /// Use reflection to create an instance of a view with passed parameter
    /// </summary>
    /// <param name="parameter">ViewModel class</param>
    /// <typeparam name="T">A View/UserControl which is going to be invoked.</typeparam>
    /// <typeparam name="TParameter">A ViewModel class as a parameter.</typeparam>
    /// <returns>Return view with already passed parameter which is ViewModel.</returns>
    /// <exception cref="MissingMethodException">
    /// If constructor does not have or have a wrong type on parameters exception will indicate the
    /// developer that there is something wrong.
    /// </exception>
    /// "where T : class" tells C# that a generic must be a class
    public T CreateViewModel<T, TParameter>(TParameter parameter) where T : class
    {
        var serviceManager = serviceProvider.GetRequiredService<IServiceManager>();
        
        // Check if parameters handled by a constructor or of a type IServiceManager and MainViewModel
        var constructorInfo = typeof(T).GetConstructor([typeof(IServiceManager), typeof(MainViewModel)]);
        
        // If not throw an exception to indicate the issue for the developer
        if (constructorInfo == null)
            throw new MissingMethodException($"Constructor on type '{typeof(T).Name}' not found.");
        
        // Invoke new view with parameterss
        return (T)constructorInfo.Invoke([serviceManager, parameter]);
    }
    
    public T CreateViewModelWithParameter<T, TParameter>(TParameter parameter) where T : class
    {
        var serviceManager = serviceProvider.GetRequiredService<IServiceManager>();
        
        // Check if parameters handled by a constructor or of a type IServiceManager and MainViewModel
        var constructorInfo = typeof(T).GetConstructor([typeof(IServiceManager), typeof(TParameter)]);
        if(constructorInfo == null)
            throw new MissingMethodException($"Constructor on type '{typeof(T).Name}' not found.");
        
        // Invoke new view with parameterss
        return (T)constructorInfo.Invoke([serviceManager, parameter]);
    }
}