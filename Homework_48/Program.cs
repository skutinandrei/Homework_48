using Dapper;
using Npgsql;
using System.Data;

namespace Homework_48
{
    internal class Program
    {
        private static string connectionString = "Host=localhost;Database=shop;Username=postgres;Password=set_password_here";

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
                    Console.WriteLine($"{customer.id} {customer.first_name} {customer.last_name} {customer.age}");
                }

                Console.Write("Введите возраст для фильтрации клиентов: ");
                int ageFilter = int.Parse(Console.ReadLine());
                var filteredCustomers = db.Query<Customer>("SELECT * FROM customer WHERE age > @age", new { age = ageFilter });
                Console.WriteLine($"Клиенты старше {ageFilter}:");
                foreach (var customer in filteredCustomers)
                {
                    Console.WriteLine($"{customer.id} {customer.first_name} {customer.last_name} {customer.age}");
                }

                // Примеры запросов к таблице Products
                var products = db.Query<Product>("SELECT * FROM product");
                Console.WriteLine("Все продукты:");
                foreach (var product in products)
                {
                    Console.WriteLine($"{product.id} {product.name} {product.price}");
                }

                Console.Write("Введите минимальную цену для фильтрации продуктов: ");
                decimal priceFilter = decimal.Parse(Console.ReadLine());
                var filteredProducts = db.Query<Product>("SELECT * FROM product WHERE price > @price", new { price = priceFilter });
                Console.WriteLine($"Продукты дороже {priceFilter}:");
                foreach (var product in filteredProducts)
                {
                    Console.WriteLine($"{product.id} {product.name} {product.price}");
                }

                // Примеры запросов к таблице Orders
                var orders = db.Query<Order>("SELECT * FROM public.order");
                Console.WriteLine("Все заказы:");
                foreach (var order in orders)
                {
                    Console.WriteLine($"{order.id} {order.customer_id} {order.product_id} {order.quantity}");
                }

                Console.Write("Введите id продукта для фильтрации заказов: ");
                int productIdFilter = int.Parse(Console.ReadLine());
                var filteredOrders = db.Query<Order>("SELECT * FROM public.order WHERE product_id = @productId", new { ProductID = productIdFilter });
                Console.WriteLine($"Заказы для продукта с ID {productIdFilter}:");
                foreach (var order in filteredOrders)
                {
                    Console.WriteLine($"{order.id} {order.customer_id} {order.product_id} {order.quantity}");
                }

                // Запрос для получения пользователей старше 30 лет, у которых есть заказ на продукт с id=1
                var query = @"
                SELECT 
                    c.id AS customer_id,
                    c.first_name AS first_name,
                    c.last_name AS last_name,
                    p.id AS product_id,
                    o.quantity AS product_quantity,
                    p.price AS product_price
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
                    Console.WriteLine($"{row.customer_id} {row.first_name} {row.last_name} {row.product_id} {row.product_quantity} {row.product_price}");
                }
            }
        }
    }
}
