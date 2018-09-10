using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        #region Attributes

        private ApiService apiService;
        private bool isRefreshing;
        public string Email { get; set; }
        public string Password { get; set; }
        
        #endregion

        #region Properties

        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }
        public Boolean IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        
        #endregion

        #region Constructors

        public ProductsViewModel()
        {
            instance = this;
            this.Email = "fernando@gmail.com";
            this.Password = "123456";
            this.apiService = new ApiService();
            this.LoadProducts();
        }

        #endregion

        #region Singleton

        private static ProductsViewModel instance;

        public static ProductsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProductsViewModel();
            }
            return instance;
        }

        #endregion

        #region Methods

        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var token = await this.apiService.GetToken(
                url,
                this.Email,
                this.Password);

            var response = await this.apiService.GetList<Product>(
                url,
                prefix,
                controller,
                token.TokenType,
                token.AccessToken);

            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            var list = (List<Product>)response.Result;
            Products = new ObservableCollection<Product>(list);
            this.IsRefreshing = false;
        }
        
        #endregion

        #region Commands

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadProducts);
            }
        }

        #endregion


    }
}
