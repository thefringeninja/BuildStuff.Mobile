using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildStuff.Mobile.ViewModels;
using ReactiveUI;
using Xamarin.Forms;

namespace BuildStuff.Mobile.Views
{
    public class ReactiveContentPage<TViewModel> : ContentPage, IViewFor<TViewModel>
        where TViewModel : class
    {
        /// <summary>
        /// The ViewModel to display
        /// </summary>
        public TViewModel ViewModel
        {
            get { return (TViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly BindableProperty ViewModelProperty =
            BindableProperty.Create<ReactiveContentPage<TViewModel>, TViewModel>(x => x.ViewModel, default(TViewModel), BindingMode.OneWay);

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TViewModel)value; }
        }
    }
	public partial class SpeakersListView : ReactiveContentPage<SpeakersListViewModel>
	{
		public SpeakersListView ()
		{
		}
	}
}
