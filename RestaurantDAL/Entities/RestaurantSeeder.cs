using RestaurantDAL.Entities;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        { 
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }
            }
        }

        public IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description = "Kentucky Fired Chicken",
                    ContactEmail =" kfc@mail.com",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Chicken nuggets",
                            Price = 20.4M,
                        },
                        new Dish()
                        {
                            Name = "Fried Chicken",
                            Price = 10.3M,
                        }
                    },
                    Address = new Address()
                    {
                        City = "Warszawa",
                        Street = "Marszałkowska 52",
                        PostalCode = "02-262",
                    }
                },
                new Restaurant()
                {
                    Name = "Mc Donalds",
                    Category = "Fast Food",
                    Description = "American burgers",
                    ContactEmail = "Mc@mail.com",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Chicken nuggets",
                            Price = 20.0M,
                        },
                        new Dish()
                        {
                            Name = "Big Mac",
                            Price = 12.0M,
                        }
                    },
                    Address = new Address()
                    {
                        City = "Warszawa",
                        Street = "Marszałkowska 10",
                        PostalCode = "02-262",
                    }
                }
            };
            return restaurants;
        }

        public IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Administrator"
                }
            };
            return roles;
        }
    }
}
