using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FreeSpace
{
	/// <summary>
	/// Interaction logic for LoadingAniControl.xaml
	/// </summary>
	public partial class LoadingAniControl : UserControl
	{
		public LoadingAniControl()
		{
			InitializeComponent();

		    var ani = (Storyboard)Resources["LoadAnimation"];
            ani.RepeatBehavior = RepeatBehavior.Forever;
            ani.Begin();
		}
	}
}