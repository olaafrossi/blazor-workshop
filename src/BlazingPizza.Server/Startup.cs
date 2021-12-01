using System.Collections.Generic;
using BlazingPizza.Server.Hubs;
using BlazingPizza.Server.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using Stripe.Checkout;

public class StripeOptions
{
    public string option { get; set; }
}

namespace BlazingPizza.Server
{
    [Route("create-checkout-session")]
    [ApiController]
    public class CheckoutApiController : Controller
    {
        [HttpPost]
        public ActionResult Create()
        {
            var domain = "https://localhost";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        // Provide the exact Price ID (e.g. pr_1234) of the product you want to sell
                        Price = "price_1K1sqUBE4TQWh8TUm7D3dnBJ",
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = domain + "/success.html",
                CancelUrl = domain + "/cancel.html",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
    }
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddMvc().AddNewtonsoftJson();

            services.AddDbContext<PizzaStoreContext>(options =>
                options.UseSqlite("Data Source=pizza.db"));

            services.AddDefaultIdentity<PizzaStoreUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<PizzaStoreContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<PizzaStoreUser, PizzaStoreContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddSignalR(options => options.EnableDetailedErrors = true)
                .AddMessagePackProtocol();

            services.AddHostedService<OrderStatusService>();
            services.AddSingleton<IBackgroundOrderQueue, DefaultBackgroundOrderQueue>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // stripe info
            StripeConfiguration.ApiKey = "sk_test_51K1skiBE4TQWh8TUM6VwNj8mEZUOO5aAm38zh4ij8YtF808vs2lxtwZWgMJsmlG3XgvlzMKsDCl9c78fIbKX3DFd00gVO3c7Oi";
            
            app.UseRouting();
            app.UseStaticFiles();
            

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<OrderStatusHub>("/orderstatus");
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
