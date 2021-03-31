using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChurchPlus
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public Command RefreshCommand { get; set; }


        public MainPage()
        {
            InitializeComponent();

            var mainViewModel = new MainViewModel();
            mainViewModel.Refreshing += MainViewModel_Refreshing;
            this.BindingContext = mainViewModel;

            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;

            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!CrossConnectivity.Current.IsConnected)
            {
                Application.Current.MainPage = new ErrorPage();
            }
            else
            {
                Webview.Source = "https://my.churchplus.co/";
            }
        }

        private void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (!e.IsConnected)
            {
                Application.Current.MainPage = new ErrorPage();
            }
            else
            {
                Webview.Source = "https://my.churchplus.co/";
            }
        }

        private void MainViewModel_Refreshing(object sender, MainViewModel e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Webview.Source = (Webview.Source as UrlWebViewSource).Url;
                this.progress.IsRefreshing = false;
            });
        }



        protected void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            progress.IsVisible = true;
        }

        protected override bool OnBackButtonPressed()
        {
            if (Webview.CanGoBack)
            {
                Webview.GoBack();
                return true;
            }
            
            return false;
        }

        protected void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            progress.IsVisible = false;
        }

       
    }
}
