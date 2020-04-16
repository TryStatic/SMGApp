﻿using System;
using System.Collections.Generic;
using System.Text;
using SMGApp.WPF.States.Navigators;

namespace SMGApp.WPF.ViewModels.Factories
{
    public interface ISMGAppViewModelAbstractFactory
    {
        ViewModelBase CreateViewModel(ViewType viewType);
    }
}
