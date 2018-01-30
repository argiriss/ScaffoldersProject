using ScaffoldersProject.Services.PaypalObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Services
{
    public interface IWebApiFetch
    {
        Task<string> WebApiFetchAsync(string path);
        Task<TokenRes> PaypalToken(string url);
        Task<PaymentIdRes> CreatePayment(string token,string getAmount);
        Task<string> ExecutePayment(string token, string paymentId, string payerId);
    }
}
