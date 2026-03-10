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
                var domain = "https://localhost:44361/";
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = orderItems.Select(oi => new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(oi.Package.Price * 100),
                            Currency = "egp",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = oi.Package.Product.Name,
                                Description = oi.Package.Product.Description,
                                Images = oi.Package.Product.Image != null
                  ? new List<string> { oi.Package.Product.Image.Url }
                  : new List<string>(),
                            },
                        },
                        Quantity = oi.Quantity,
                    }).ToList(),
                    Mode = "payment",
                    SuccessUrl = domain + "/confirm",
                    CancelUrl = domain + "/deny",
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
