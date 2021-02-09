using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
            try
            {
                InitializeComponent();
                
                //Avoid white flickering of big background image
                this.WindowState = WindowState.Minimized;
                Loaded += (_, __) => { this.WindowState = WindowState.Maximized; };
            }
            catch (Exception ex)
            {
                throw;
            }
		}
    }
}
