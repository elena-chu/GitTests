using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Ws.Fus.ImageViewer.UI.Wpf;

namespace WpfUI.Controls
{
    /// <summary>
    /// ProgressNavigationButton can reflect different aspects of navigation stages.
    /// </summary>
    [TemplateVisualState(Name = "Available", GroupName = "StatusStates")]
    [TemplateVisualState(Name = "Unavailable", GroupName = "StatusStates")]
    [TemplateVisualState(Name = "Completed", GroupName = "StatusStates")]
    [TemplateVisualState(Name = "Active", GroupName = "ActiveStates")]
    [TemplateVisualState(Name = "ActiveCompleted", GroupName = "ActiveStates")]
    [TemplateVisualState(Name = "Inactive", GroupName = "ActiveStates")]
    [TemplateVisualState(Name = "CallToActionActive", GroupName = "CallToActionStates")]
    [TemplateVisualState(Name = "CallToActionInactive", GroupName = "CallToActionStates")]
    public class ProgressNavigationButton : Button
    {
        static ProgressNavigationButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressNavigationButton), new FrameworkPropertyMetadata(typeof(ProgressNavigationButton)));
        }

        public ProgressNavigationButton() : base()
        {
            Loaded += (_, __) => UpdateStates(); ;
            IsEnabledChanged += OnIsEnabledChanged;
        }

        #region Dependency Properties

        /// <summary>
        /// ProgressNavigationState property notifies about progress state.
        /// Available values: Regular, Active, Completed
        /// For disabled state use standard IsEnabled property of Control.
        /// </summary>
        public static readonly DependencyProperty ProgressNavigationStateProperty = DependencyProperty.Register("ProgressNavigationState",
            typeof(ProgressNavigationState), typeof(ProgressNavigationButton), new UIPropertyMetadata(ProgressNavigationState.Available, OnProgressNavigationStateChanged));

        public static ProgressNavigationState GetProgressNavigationState(DependencyObject obj)
        {
            return (ProgressNavigationState)obj.GetValue(ProgressNavigationStateProperty);
        }

        public static void SetProgressNavigationState(DependencyObject obj, ProgressNavigationState value)
        {
            obj.SetValue(ProgressNavigationStateProperty, value);
        }

        private static void OnProgressNavigationStateChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ProgressNavigationButton fe = sender as ProgressNavigationButton;
            if (fe == null)
                return;
            fe.SetProgressNavigationState();
        }

        /// <summary>
        /// IsActive property notifies about current activity
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive",
            typeof(bool), typeof(ProgressNavigationButton), new UIPropertyMetadata(false, OnIsActiveChanged));

        public static bool GetIsActive(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsActiveProperty);
        }

        public static void SetIsActive(DependencyObject obj, bool value)
        {
            obj.SetValue(IsActiveProperty, value);
        }

        private static void OnIsActiveChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ProgressNavigationButton fe = sender as ProgressNavigationButton;
            if (fe == null)
                return;

            fe.SetActiveState();
        }

        /// <summary>
        /// CallToAction property notifies to start CallToAction animation
        /// </summary>
        public static readonly DependencyProperty CallToActionProperty = DependencyProperty.Register("CallToAction",
            typeof(bool), typeof(ProgressNavigationButton), new UIPropertyMetadata(false, CallToActionChanged));

        public static bool GetCallToAction(DependencyObject obj)
        {
            return (bool)obj.GetValue(CallToActionProperty);
        }

        public static void SetCallToAction(DependencyObject obj, bool value)
        {
            obj.SetValue(CallToActionProperty, value);
        }

        private static void CallToActionChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ProgressNavigationButton fe = sender as ProgressNavigationButton;
            if (fe == null)
                return;

            bool active = (bool)e.NewValue;
            string stateName = active ? "CallToActionActive" : "CallToActionInactive";
            fe.GoToState(stateName, true);
        }

        #endregion

        private void UpdateStates()
        {
            SetProgressNavigationState();
            SetActiveState();
            SetCallToActionState();
        }

        private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool isEnabled = (bool)e.NewValue;
            if (!isEnabled)
            {
                GoToState("CallToActionInactive", true);
            }
            else
                UpdateStates();
        }

        private void SetProgressNavigationState()
        {
            string stateName = GetProgressNavigationState(this).ToString();
            GoToState(stateName, true);
        }

        private void SetActiveState()
        {
            bool active = GetIsActive(this);
            string stateName = active ? GetProgressNavigationState(this) == ProgressNavigationState.Completed ? "ActiveCompleted" : "Active" : "Inactive";
            GoToState(stateName, true);
        }

        private void SetCallToActionState()
        {
            bool active = GetCallToAction(this);
            string stateName = active ? "CallToActionActive" : "CallToActionInactive";
            GoToState(stateName, true);
        }

        private void GoToState(string stateName, bool useTransition)
        {
            VisualStateManager.GoToState(this, stateName, true);
        }

    }
}
