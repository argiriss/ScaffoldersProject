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

        public async Task<string> WebApiFetchAsync(string url, string path)
        {
            try
            {
                client.BaseAddress = new Uri(url);
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
            finally
            {
                // Need to call dispose on the HttpClient object
                // when done using it, so the app doesn't leak resources
                client.Dispose();
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
                var accessToken=serializer.ReadObject(responseBody) as TokenRes;
                return accessToken;
            }
            catch (HttpRequestException e)
            {
                var error = $"Message : {e.Message}";
                return new TokenRes {
                    access_token=error
                };
            }
        }

        public async Task<string> CreatePayment(string token)
        {
            //Add headers
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var serializePaymentReq = new DataContractJsonSerializer(typeof(PaymentReq));
            HttpContent _body = new StringContent(serializePaymentReq, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://api.sandbox.paypal.com/v1/payments/payment", _body);

            //Response           
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;



        }
    }
}
