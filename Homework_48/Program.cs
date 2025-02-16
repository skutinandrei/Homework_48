using Dapper;
using Npgsql;
using System.Data;

namespace Homework_48
{
    internal class Program
    {
        private static string connectionString = "Host=localhost;Database=shop;Username=postgres;Password=put_password_here";

        static void Main(string[] args)
        {
            using (IDbConnection db = new NpgsqlConnection(connectionString))
            {
                db.Open();

                // Примеры запросов к таблице Customers
                var customers = db.Query<Customer>("SELECT * FROM customer");
                Console.WriteLine("Все клиенты:");
                foreach (var customer in customers)
                {
                    Console.WriteLine($"{customer.Id} {customer.FirstName} {customer.LastName} {customer.Age}");
                }

                Console.Write("Введите возраст для фильтрации клиентов: ");
                int ageFilter = int.Parse(Console.ReadLine());
                var filteredCustomers = db.Query<Customer>("SELECT * FROM customer WHERE age > @age", new { age = ageFilter });
                Console.WriteLine($"Клиенты старше {ageFilter}:");
                foreach (var customer in filteredCustomers)
                {
                    Console.WriteLine($"{customer.Id} {customer.FirstName} {customer.LastName} {customer.Age}");
                }

                // Примеры запросов к таблице Products
                var products = db.Query<Product>("SELECT * FROM product");
                Console.WriteLine("Все продукты:");
                foreach (var product in products)
                {
                    Console.WriteLine($"{product.Id} {product.Name} {product.Price}");
                }

                Console.Write("Введите минимальную цену для фильтрации продуктов: ");
                decimal priceFilter = decimal.Parse(Console.ReadLine());
                var filteredProducts = db.Query<Product>("SELECT * FROM product WHERE price > @price", new { price = priceFilter });
                Console.WriteLine($"Продукты дороже {priceFilter}:");
                foreach (var product in filteredProducts)
                {
                    Console.WriteLine($"{product.Id} {product.Name} {product.Price}");
                }

                // Примеры запросов к таблице Orders
                var orders = db.Query<Order>("SELECT * FROM public.order");
                Console.WriteLine("Все заказы:");
                foreach (var order in orders)
                {
                    Console.WriteLine($"{order.Id} {order.CustomerId} {order.ProductId} {order.Quantity}");
                }

                Console.Write("Введите id продукта для фильтрации заказов: ");
                int productIdFilter = int.Parse(Console.ReadLine());
                var filteredOrders = db.Query<Order>("SELECT * FROM public.order WHERE product_id = @productId", new { ProductID = productIdFilter });
                Console.WriteLine($"Заказы для продукта с ID {productIdFilter}:");
                foreach (var order in filteredOrders)
                {
                    Console.WriteLine($"{order.Id} {order.CustomerId} {order.ProductId} {order.Quantity}");
                }

                // Запрос для получения пользователей старше 30 лет, у которых есть заказ на продукт с id=1
                var query = @"
                SELECT 
                    c.id AS CustomerId,
                    c.first_name AS FirstName,
                    c.last_name AS LastName,
                    p.id AS ProductId,
                    o.quantity AS ProductQuantity,
                    p.price AS ProductPrice
                FROM 
                    customer c
                JOIN 
                    public.order o ON c.id = o.customer_id
                JOIN 
                    product p ON o.product_id = p.id
                WHERE 
                    c.age > 30 AND p.id = 1;";

                var result = db.Query<CustomerOrderResult>(query);

                Console.WriteLine("Список пользователей старше 30 лет, у которых есть заказ на продукт с ID=1:");
                foreach (var row in result)
                {
                    Console.WriteLine($"{row.CustomerId} {row.FirstName} {row.LastName} {row.ProductId} {row.ProductQuantity} {row.ProductPrice}");
                }
            }
        }
    }
}
