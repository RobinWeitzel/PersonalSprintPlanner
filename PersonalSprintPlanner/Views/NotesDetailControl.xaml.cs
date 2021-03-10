using System;

using PersonalSprintPlanner.Core.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PersonalSprintPlanner.Views
{
    public sealed partial class NotesDetailControl : UserControl
    {
        public SampleOrder MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(NotesDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public NotesDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as NotesDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
