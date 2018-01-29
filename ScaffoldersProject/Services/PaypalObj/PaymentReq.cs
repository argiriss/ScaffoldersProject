using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Services.PaypalObj
{
    public class PaymentReq
    {
        public string intent { get; set; } = "sale";

        public Redirect_Urls redirect_urls { get; set; } = new Redirect_Urls
        {
            return_url="http://localhost:61971/Client/success",
            cancel_url= "http://localhost:61971/Client/cancel"
        };

        public Payer payer { get; set; } = new Payer
        {
            payment_method="paypal"
        };

        public Transaction[] transactions { get; set; } = new Transaction[] { };
    
        public class Redirect_Urls
        {
            public string return_url { get; set; }
            public string cancel_url { get; set; }
        }

        public class Payer
        {
            public string payment_method { get; set; }
        }

        public class Transaction
        {
            public Amount amount { get; set; } = new Amount();
        
        }

        public class Amount
        {
            public string total { get; set; } = "10";
            public string currency { get; set; } = "USD";
        }

    }
}
