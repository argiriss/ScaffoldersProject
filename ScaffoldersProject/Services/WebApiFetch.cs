using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using ScaffoldersProject.Services.PaypalObj;
using System.IO;

namespace ScaffoldersProject.Services
{
    public class WebApiFetch : IWebApiFetch
    {
        //HttpClient is intended to be instantiated once and re-used throughout the life 
        //of an application.Instantiating an HttpClient class for every request will exhaust 
        //the number of sockets available under heavy loads.This will result in 
        //SocketException errors. Below is an example using HttpClient correctly.
        private static readonly HttpClient client;
        public IConfiguration _iconfiguration;

        static WebApiFetch()
        {
            client = new HttpClient();
        }

        //Constructor depedency injection for iconfiguration so as to read from 
        //appsetings.json
        public WebApiFetch(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        public async Task<string> WebApiFetchAsync(string path)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(path);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                var error = $"Message : {e.Message}";
                return error;
            } 
        }

        public async Task<TokenRes> PaypalToken(string url)
        {
            try
            {
                //Add headers
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
                client.DefaultRequestHeaders.Add("Accept-Language", "en_Us");

                //Authentication Basic
                var clientId = _iconfiguration.GetValue<string>("Paypal:clientId");
                var secret = _iconfiguration.GetValue<string>("Paypal:clientSecret");
                var byteArray = Encoding.ASCII.GetBytes($"{clientId}:{secret}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                HttpContent _body = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
                HttpResponseMessage response = await client.PostAsync(url, _body);

                //Response
                var serializer = new DataContractJsonSerializer(typeof(TokenRes));
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStreamAsync();
                var accessToken = serializer.ReadObject(responseBody) as TokenRes;
                return accessToken;
            }
            catch (HttpRequestException e)
            {
                var error = $"Message : {e.Message}";
                return new TokenRes
                {
                    access_token = error
                };
            }
        }

        public async Task<PaymentIdRes> CreatePayment(string token,string getAmount)
        {
            //Add headers
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Payment object
            PaymentObj pObj = new PaymentObj
            {
                intent = "sale",
                payer=new PaymentObj.Payer
                {
                    payment_method="paypal"
                },
                redirect_urls=new PaymentObj.Redirect_Urls
                {
                    return_url= "http://localhost:61971/Client/Success",
                    cancel_url= "http://localhost:61971/Client/Cancel"
                },
                transactions=new PaymentObj.Transaction[]
                {
                    new PaymentObj.Transaction
                    {
                        amount=new PaymentObj.Amount
                        {
                            total=getAmount,
                            currency="EUR"
                        }
                    }
                }
            };

            DataContractJsonSerializer serializePaymentObj = new DataContractJsonSerializer(typeof(PaymentObj));
            //Create a stream to serialize the object to. 
            MemoryStream ms = new MemoryStream();
            serializePaymentObj.WriteObject(ms, pObj);
            byte[] json = ms.ToArray();
            ms.Close();
            string bodyToSend = Encoding.UTF8.GetString(json, 0, json.Length);
            HttpContent _body = new StringContent(bodyToSend, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://api.sandbox.paypal.com/v1/payments/payment", _body);

            //Response  
            var serializer = new DataContractJsonSerializer(typeof(PaymentIdRes));
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStreamAsync();
            var payId = serializer.ReadObject(responseBody) as PaymentIdRes;
            return payId;
        }

        public async Task<string> ExecutePayment(string token,string paymentId,string payerId)
        {
            var urlToSent = $"https://api.sandbox.paypal.com/v1/payments/payment/{paymentId}/execute";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            PayerId payId = new PayerId
            {
                payer_id = payerId
            };
            //Serializer the Payer object to the stream.
            DataContractJsonSerializer serializePayerId = new DataContractJsonSerializer(typeof(PayerId));
            //Create a stream to serialize the object to. 
            MemoryStream ms = new MemoryStream();
            serializePayerId.WriteObject(ms, payId);
            byte[] json = ms.ToArray();
            ms.Close();
            string bodyToSend= Encoding.UTF8.GetString(json, 0, json.Length);

            HttpContent _body = new StringContent(bodyToSend, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(urlToSent, _body);

            //Response  
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
