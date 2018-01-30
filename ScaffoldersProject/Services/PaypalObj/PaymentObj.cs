using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Services.PaypalObj
{
    public class PaymentObj
    {
            public string intent { get; set; }
            public Redirect_Urls redirect_urls { get; set; }
            public Payer payer { get; set; }
            public Transaction[] transactions { get; set; }
        

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
            public Amount amount { get; set; }
        }

        public class Amount
        {
            public string total { get; set; }
            public string currency { get; set; }
        }

    }
}
