using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace AmazonCloneMVC.Models
{

    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public static class SessionExtensions 
    {
        public static void SetCartService(this ISession session, CartService cartService)
        {
            string cartServiceJson = JsonConvert.SerializeObject(cartService);
            session.SetString("CartService", cartServiceJson);
        }

        public static CartService GetCartService(this ISession session)
        {
            string cartServiceJson = session.GetString("CartService");
            if (cartServiceJson != null)
            {
                return JsonConvert.DeserializeObject<CartService>(cartServiceJson);
            }
            return new CartService(); // Return  if the object is not found in session
        }


    }
   

}
