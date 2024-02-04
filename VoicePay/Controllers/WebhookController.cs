using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;



namespace VoicePay.Controllers
{
    [ApiController]
    [Route("webhook")]
    public class WebhookController : Controller
    {
        private static bool paymentProcessed = false;
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            Debug.WriteLine("HI");

            if (!string.IsNullOrEmpty(json))
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], "whsec_5FdaxzbIsciKjAyZi1vZDW3QRX9gXqRB");

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    // Handle the completed Checkout Session
                    var session = stripeEvent.Data.Object as Session;
                    Debug.WriteLine("Checkout session completed. Sending success response.");
                    paymentProcessed = true;
                }
            }

            // Respond to the webhook request with a 2xx status code
            return Ok();
        }

        [HttpGet("CheckFlag")]
        public IActionResult CheckFlag()
        {
            return Json(new { PaymentProcessed = paymentProcessed });
        }

        [HttpPost("Reset")]
        public IActionResult Reset()
        {
            paymentProcessed = false;
            return Ok();
        }
    }
}
