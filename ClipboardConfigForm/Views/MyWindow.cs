using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ClipboardConfigForm.Views
{
    public class MyWindow : Window
    {
        public static readonly DependencyProperty BusyProperty =
    DependencyProperty.Register(nameof(Busy), typeof(bool), typeof(MyWindow),
        new UIPropertyMetadata(false, OnBusyChangedCallBack));
        public bool CloseWhenNotBusy { get; set; } = false;

        public bool Busy
        {
            get { return (bool)GetValue(BusyProperty); }
            set
            {
                bool oldValue = (bool)GetValue(BusyProperty);
                if (oldValue != value)
                {
                    SetValue(BusyProperty, value);
                }
            }
        }
        private static void OnBusyChangedCallBack(
        DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var myWindow = sender as MyWindow;
            if (myWindow != null)
            {
                myWindow.OnBusyChanged(e);
            }
        }

        protected virtual void OnBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (CloseWhenNotBusy && (bool)e.NewValue == false)
            {
                this.Close();
            }
        }
    }
}
