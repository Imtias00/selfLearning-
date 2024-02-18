using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly string connectionString;
        public ProductController(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SqlServerDb"] ?? "";
        }
        [HttpPost]
        public IActionResult CreateProduct(ProductDto productDto)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "INSERT INTO products " + "(name,brand,catagory,price,description) VALUES " + "(@name, @brand,@catagory,@price,@description)";
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@name", productDto.Name);
                command.Parameters.AddWithValue("@brand", productDto.Brand);
                command.Parameters.AddWithValue("@catagory", productDto.Catagory);
                command.Parameters.AddWithValue("@price", productDto.Price);
                command.Parameters.AddWithValue("@description", productDto.Description);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Product", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok();
        }
        [HttpGet]
        public IActionResult GetProduct()
        {
            List<Product> products = new List<Product>();
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "SELECT * FROM products";
                using var command = new SqlCommand(sql, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product();
                    product.Id = reader.GetInt32(0);
                    product.Name = reader.GetString(1);
                    product.Brand = reader.GetString(2);
                    product.Catagory = reader.GetString(3);
                    product.Price = reader.GetDecimal(4);
                    product.Description = reader.GetString(5);
                    product.CreatedAt = reader.GetDateTime(6);

                    products.Add(product);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("Product", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }

            return Ok(products);
        }
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id) {
            Product product = new Product();
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "SELECT * FROM products WHERE id = @id";
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    product.Id = reader.GetInt32(0);
                    product.Name = reader.GetString(1);
                    product.Brand = reader.GetString(2);
                    product.Catagory = reader.GetString(3);
                    product.Price = reader.GetDecimal(4);
                    product.Description = reader.GetString(5);
                    product.CreatedAt = reader.GetDateTime(6);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("Product", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok(product);
        }
        [HttpPut("{id}")]
        public IActionResult UpadateProduct(int id, ProductDto productDto)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string sql = "UPDATE products SET name=@name, brand=@brand, catagory=@catagory, price=@price, description=@description WHERE id=@id";
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@name", productDto.Name);
                command.Parameters.AddWithValue("@brand", productDto.Brand);
                command.Parameters.AddWithValue("@catagory", productDto.Catagory);
                command.Parameters.AddWithValue("@price", productDto.Price);
                command.Parameters.AddWithValue("@description", productDto.Description);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Product", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "DELETE FROM products WHERE id=@id";
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Product", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok();
        }
    }
}
