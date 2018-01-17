using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ScaffoldersProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Services
{
    public class SessionCart : Cart
    {
        private static ISession session;
        private const string sessionKey = "cart";

        public static Cart GetCart(IServiceProvider services)
        {
            session = services.GetRequiredService<IHttpContextAccessor>().HttpContext.Session;
            var returnData = session.GetString(sessionKey);
            if (returnData == null)
            {
                return new SessionCart();
            }
            else
            {
                return JsonConvert.DeserializeObject<SessionCart>(returnData);
            }
        }

        public override void AddProduct(Products product, int quantity)
        {
            base.AddProduct(product, quantity);
            session.SetString(sessionKey, JsonConvert.SerializeObject(this));
        }

        public override void Clear()
        {
            base.Clear();
            session.Remove(sessionKey);
        }
    }
}

