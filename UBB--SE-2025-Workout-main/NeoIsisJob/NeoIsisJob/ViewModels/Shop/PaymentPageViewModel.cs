// <copyright file="PaymentPageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NeoIsisJob.ViewModels.Shop
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using NeoIsisJob.Proxy;


    /// <summary>
    /// Represents the view model for the payment page.
    /// </summary>
    public class PaymentPageViewModel
    {
        private readonly OrderServiceProxy orderServiceProxy;
        private readonly int userId = AppController.CurrentUser != null ? AppController.CurrentUser.ID : 1; // This should be replaced with the actual user ID from the session or authentication context.

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentPageViewModel"/> class.
        /// </summary>
        public PaymentPageViewModel()
        {
            this.orderServiceProxy = new OrderServiceProxy();
        }

        /// <summary>
        /// Sends the order by creating it from the cart.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task<bool> SendOrder()
        {
            try
            {
                var result = await this.orderServiceProxy.CreateOrderFromCartAsync(this.userId);
                return result != null;
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error creating order: {exception.Message}");
                return false;
            }
        }
    }
}