using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.BL.Service.IService;
using ThanyaProject.Models.Model;

namespace ThanyaProject.BL.Service
{
    public class StripeService : IStripeService
    {
        public async Task<string> CreateCheckoutSession(IEnumerable<OrderItem> orderItems, string successUrl)
        {
            try
            {
                var domain = "http://thanyaproject.runasp.net/";
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = orderItems.Select(oi => new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(oi.Price * 100),
                            Currency = "egp",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = oi.Product.Name,
                                Description = oi.Product.Description,
                                Images = oi.Product.Image != null
                                ? new List<string> { oi.Product.Image.Url }
                                : new List<string>(),
                            },
                        },
                        Quantity = oi.Quantity,
                    }).ToList(),
                    Mode = "payment",
                    SuccessUrl = successUrl,
                    CancelUrl = domain + "/swagger/index.html",
                };

                var service = new SessionService();
                Session session = await service.CreateAsync(options);

                return session.Url;
            }
            catch (Exception)
            {
                throw new Exception("An error occurred during checkout.");
            }
        }
    }
}
