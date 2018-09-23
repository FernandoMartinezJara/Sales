using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using Sales.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class ProductItemViewModel : Product
    {
        #region Attributes

        private ApiService apiService;

        #endregion

        #region Constructor

        public ProductItemViewModel()
        {
            this.apiService = new ApiService();
        }

        #endregion

        #region Command

        public ICommand DeleteProductCommand
        {
            get
            {
                return new RelayCommand(DeleteProduct);
            }
        }


        public ICommand EditProductCommand
        {
            get
            {
                return new RelayCommand(EditProduct);
            }
        }
        #endregion

        #region Methods

        private async void DeleteProduct()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.DeleteConfirmation,
                Languages.Yes,
                Languages.No);

            if (!answer)
            {
                return;
            }

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
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
                "fernando@gmail.com",
                "123456");

            var response = await this.apiService.Delete(
              url,
              prefix,
              controller,
              token.TokenType,
              token.AccessToken,
              this.ProductId);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert
(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            var productViewModel = ProductsViewModel.GetInstance();
            var deletedProduct = productViewModel.Products.Where(x => x.ProductId == this.ProductId).FirstOrDefault();
            if (deletedProduct != null)
            {
                productViewModel.Products.Remove(deletedProduct);
            }


        }

        private async void EditProduct()
        {
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditProductPage());
        }

        #endregion
    }
}
