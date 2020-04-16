﻿using System;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using SMGApp.Domain.Models;
using SMGApp.EntityFramework;
using SMGApp.EntityFramework.Services;
using SMGApp.WPF.States.Navigators;
using SMGApp.WPF.ViewModels;
using SMGApp.WPF.ViewModels.Factories;

namespace SMGApp.WPF.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private INavigator _navigator;
        private readonly ISMGAppViewModelAbstractFactory _viewModelFactory;

        public UpdateCurrentViewModelCommand(INavigator navigator, ISMGAppViewModelAbstractFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (!(parameter is ViewType viewType)) return;

            _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
        }

    }
}
