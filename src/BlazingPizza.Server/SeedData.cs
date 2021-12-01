using System.Threading.Tasks;

namespace BlazingPizza.Server
{
    public static class SeedData
    {
        public static Task InitializeAsync(PizzaStoreContext db)
        {
            var toppings = new Topping[]
            {
                new Topping
                {
                    Name = "Extra Sauce",
                    Price = 1m,
                },
                new Topping
                {
                    Name = "Extra Onions",
                    Price = 1m,
                },
                new Topping
                {
                    Name = "Extra Peppers",
                    Price = 1m,
                },
            };

            var specials = new PizzaSpecial[]
            {
                new PizzaSpecial
                {
                    Name = "Sandwich1",
                    Description = "Tastes like chicken",
                    BasePrice = 1m,
                    ImageUrl = "img/pizzas/sandwich1.jpg",
                }
            };

            db.Toppings.AddRange(toppings);
            db.Specials.AddRange(specials);

            return db.SaveChangesAsync();
        }
    }
}
