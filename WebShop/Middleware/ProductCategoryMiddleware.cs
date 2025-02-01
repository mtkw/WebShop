using System.Security.Claims;
using WebShop.DataAccess.Repository.IRepository;

namespace WebShop.Middleware
{
    //Klasa Middleware jest wykorzystana do pobrania i zapamiętania listy Kateogri która będzie dostępna z poziomu _Leyout.cshtml
    //List ta będzie przechowywana w HttpContext i dostęna z każdego poziomu aplikacji
    //Klasy Middleware wymagają odpowiedniej konfiguracji w klasie program.cs
    public class ProductCategoryMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public ProductCategoryMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }
        //Do konstruktora klasy Middleware można przekazywać tylko klasy o zasięgu Singleton dlatego IUnitOfWork które ma zasięg Scoped musi byc przekazane jako parametr w metodzie InvokeAsync. 
        // Dla interfejsów i serwisów o zasiegu innym niż Singleton nie jest możliwe ustawienie Dependecy Injection w konstruktorze klasy Middleware
        public async Task InvokeAsync(HttpContext context, IUnitOfWork _unitOfWork)
        {
            var categories = await _unitOfWork.ProductCategory.GetAllAsync();
            context.Items["CategoryList"] = categories;

            //Test dla koszyka
            var claimsIdentity = context.User.Identity as ClaimsIdentity;

            if (claimsIdentity != null && claimsIdentity.IsAuthenticated)
            {
                // You can now access claims
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userEmail = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
                var cart = await _unitOfWork.ShoppingCart.GetAllAsync(x => x.ApplicationUserId == userId, includProperties: "CartItems");
                if (cart.Count != 0) { context.Items["CartItemCounter"] = cart.First().CartCountItems; }
                else { context.Items["CartItemCounter"] = 0; }

                var messages = await _unitOfWork.UsersMessage.GetAllAsync(x => x.UserId == userId && !x.IsRead);
                if (messages.Count != 0) { context.Items["MessageCounter"] = messages.Count; }
                else { context.Items["MessageCounter"] = 0; }


            }

            await _requestDelegate(context);
        }
    }
}
