using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GardenGroup;

public class RelayCommand : ICommand
{
    // Delegate to hold the method to be executed when the command is invoked
    private readonly Action<object> _execute;

    // Delegate to hold the method that determines if the command can execute
    private readonly Func<object, bool> _canExecute;

    // Constructor that initializes the _execute and _canExecute delegates
    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _execute = execute; // Assign the execute delegate to the _execute field
        _canExecute = canExecute; // Assign the canExecute delegate to the _canExecute field
    }

    // Determines if the command can execute
    public bool CanExecute(object parameter)
    {
        // If _canExecute is null, the command can always execute
        // Otherwise, use _canExecute delegate to determine if it can execute
        return _canExecute == null || _canExecute(parameter);
    }

    // Executes the command
    public void Execute(object parameter)
    {
        // Invoke the _execute delegate to perform the command logic
        _execute(parameter);
    }

    // Event that is used to notify the UI to re-query the CanExecute status
    public event EventHandler CanExecuteChanged
    {
        // Add handler to the CommandManager's RequerySuggested event
        // This event is raised by the CommandManager when it needs to re-evaluate
        // whether the command can execute
        add { CommandManager.RequerySuggested += value; }

        // Remove handler from the CommandManager's RequerySuggested event
        remove { CommandManager.RequerySuggested -= value; }
    }
}