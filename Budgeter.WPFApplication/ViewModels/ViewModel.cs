using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace Budgeter.WPFApplication.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        private Dictionary<string, ViewModel> _childViewModelByName = new Dictionary<string, ViewModel>();
        private Dictionary<string, PropertyChangedEventHandler> _handlersByName = new Dictionary<string, PropertyChangedEventHandler>();

        protected ObservableCollection<ViewModel> _childViewModels = new ObservableCollection<ViewModel>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Contracts", "CS0067", Justification = "Fody.PropertyChanged requires this event.")]
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler ChildPropertyChanged;

        // TODO - Will this even get called via Fody since ChildViewModels is protected instead of public?
        public void OnChildViewModelsChanged()
        {
            foreach (var childViewModel in _childViewModels)
            {
                childViewModel.PropertyChanged += ChildPropertyChanged;
            }
        }

        public void AddChild(ViewModel childViewModel, PropertyChangedEventHandler changeHandler)
        {
            childViewModel.PropertyChanged += changeHandler;
            _childViewModels.Add(childViewModel);
        }

        public void RemoveChild(ViewModel childViewModel, PropertyChangedEventHandler changeHandler)
        {
            _childViewModels.Remove(childViewModel);
            childViewModel.PropertyChanged -= changeHandler;
        }

        public void InvokePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public virtual void OnPropertyChanged(string propertyName)
        {
            var propertyInfo = GetType().GetProperty(propertyName);

            if (propertyInfo.GetCustomAttribute<PropagateChangesAttribute>(true) != null)
            {
                var propertyValue = propertyInfo.GetValue(this);

                var newViewModel = propertyValue as ViewModel;
                var oldViewModel = _childViewModelByName.ContainsKey(propertyName) ? _childViewModelByName[propertyName] : null;

                if (oldViewModel != newViewModel)
                {
                    // Deregister previous view model
                    if (oldViewModel != null)
                    {
                        var childPropertyChangeHandler = _handlersByName[propertyName];
                        oldViewModel.PropertyChanged -= childPropertyChangeHandler;
                        _handlersByName.Remove(propertyName);

                        _childViewModelByName.Remove(propertyName);
                        _childViewModels.Remove(oldViewModel);
                    }

                    // Register new view model
                    if (newViewModel != null)
                    {
                        var childPropertyChangeHandler = CreateChildPropertyChangeHandler(propertyName);
                        _handlersByName.Add(propertyName, childPropertyChangeHandler);

                        _childViewModelByName[propertyName] = newViewModel;
                        newViewModel.PropertyChanged += childPropertyChangeHandler;
                        _childViewModels.Add(newViewModel);
                    }
                }
            }

            InvokePropertyChanged(propertyName);
        }

        private PropertyChangedEventHandler CreateChildPropertyChangeHandler(string propertyName) => new PropertyChangedEventHandler((s, args) =>
        {
            // This child view model should propagate any of its changing properties upward, but renamed to the encapsulating property name
            var propertyInfo = GetType().GetProperty(propertyName);

            // This is janky, but this is going to force call the setter, which should call the injected On_PropertyName_Changed fody method
            var propertyValue = propertyInfo.GetValue(this);
            propertyInfo.SetValue(this, propertyValue);

            InvokePropertyChanged(propertyName);
        });
    }
}
