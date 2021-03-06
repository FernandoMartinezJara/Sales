﻿using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        #region Attributes

        private ApiService apiService;
        private DataService dataService;
        private bool isRefreshing;
        private string filter;
        private ObservableCollection<ProductItemViewModel> products;

        #endregion

        #region Properties

        public List<Product> MyProducts { get; set; }

        public ObservableCollection<ProductItemViewModel> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }

        public Boolean IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public string Filter
        {
            get { return this.filter; }
            set
            {
                this.filter = value;
                this.RefreshList();
            }
        }

        public Category Category { get; set; }

        #endregion

        #region Constructors

        public ProductsViewModel(Category category)
        {
            instance = this;
            this.Category = category;
            this.apiService = new ApiService();
            this.dataService = new DataService();
            this.LoadProducts();
        }

        #endregion

        #region Singleton

        private static ProductsViewModel instance;
        private CategoryItemViewModel categoryItemViewModel;

        public static ProductsViewModel GetInstance()
        {
            return instance;
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

        public ICommand SearchCommad
        {
            get
            {
                return new RelayCommand(RefreshList);
            }
        }

        #endregion

        #region Methods

        //private async void LoadProducts()
        //{
        //    this.IsRefreshing = true;

        //    var connection = await this.apiService.CheckConnection();

        //    if (connection.IsSuccess)
        //    {
        //        var answer = await LoadProductsFromAPI();
        //        if (answer)
        //        {
        //            this.SaveProductsToDB();
        //        }
        //    }
        //    else
        //    {
        //        await this.LoadProductsFromDB();
        //    }

        //    if (MyProducts == null && MyProducts.Count == 0)
        //    {
        //        this.IsRefreshing = false;
        //        await Application.Current.MainPage.DisplayAlert(
        //            Languages.Error, 
        //            Languages.NoProductsMessage, 
        //            Languages.Accept);
        //        return;
        //    }

        //    this.RefreshList();

        //    this.IsRefreshing = false;
        //}

        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            var answer = await this.LoadProductsFromAPI();
            if (answer)
            {
                this.RefreshList();
            }

            this.IsRefreshing = false;
        }

        private async Task<bool> LoadProductsFromAPI()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.GetList<Product>(
                url, 
                prefix, 
                controller, 
                this.Category.CategoryId,
                Settings.TokenType, 
                Settings.AccessToken);

            if (!response.IsSuccess)
            {
                return false;
            }

            this.MyProducts = (List<Product>)response.Result;
            return true;
        }


        private async Task LoadProductsFromDB()
        {
            this.MyProducts = await dataService.GetAllProducts();
        }

        private async Task SaveProductsToDB()
        {
            await this.dataService.DeleteAllProducts();
            dataService.Insert(this.MyProducts);
        }

        //private async Task<bool> LoadProductsFromAPI()
        //{

        //    var url = Application.Current.Resources["UrlAPI"].ToString();
        //    var prefix = Application.Current.Resources["UrlPrefix"].ToString();
        //    var controller = Application.Current.Resources["UrlProductsController"].ToString();

        //    var response = await this.apiService.GetList<Product>(
        //        url,
        //        prefix,
        //        controller,
        //        Settings.TokenType,
        //        Settings.AccessToken);

        //    if (!response.IsSuccess)
        //    {
        //        return false;
        //    }

        //    MyProducts = (List<Product>)response.Result;
        //    return true;
        //}

        public void RefreshList()
        {
            if (string.IsNullOrEmpty(Filter))
            {
                var myListProductItemViewModel = this.MyProducts.Select(p => new ProductItemViewModel
                {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks,
                    CategoryId = p.CategoryId,
                    UserId = p.UserId
                });

                this.Products = new ObservableCollection<ProductItemViewModel>(
                    myListProductItemViewModel.OrderBy(p => p.Description));
            }
            else
            {
                var myListProductItemViewModel = this.MyProducts.Select(p => new ProductItemViewModel
                {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks,
                    CategoryId = p.CategoryId,
                    UserId = p.UserId
                }).Where(p => p.Description.ToLower().Contains(this.Filter.ToLower())).ToList();

                this.Products = new ObservableCollection<ProductItemViewModel>(
                    myListProductItemViewModel.OrderBy(p => p.Description));

            }
        }

        #endregion

    }
}
