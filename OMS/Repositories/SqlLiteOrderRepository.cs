using Microsoft.Data.Sqlite;

namespace OMS.Repositories
{
    public class SqlLiteOrderRepository : IOrderRepository
    {
        private readonly string connectionString;

        public SqlLiteOrderRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddOrder(Order order)
        {
            using (var connection = new SqliteConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "INSERT INTO Orders (OrderId, Account, ClOrdId, Quantity, ExecutedQuantity, Price, Status, Symbol) " +
                                      "VALUES (@OrderId, @Account, @ClOrdId, @Quantity, @ExecutedQuantity, @Price, @Status, @Symbol)";
                command.Parameters.AddWithValue("@OrderId", order.OrderId);
                command.Parameters.AddWithValue("@Account", order.Account);
                command.Parameters.AddWithValue("@ClOrdId", order.ClOrdId);
                command.Parameters.AddWithValue("@Quantity", order.Quantity);
                command.Parameters.AddWithValue("@ExecutedQuantity", order.ExecutedQuantity);
                command.Parameters.AddWithValue("@Price", order.Price);
                command.Parameters.AddWithValue("@Status", order.Status);
                command.Parameters.AddWithValue("@Symbol", order.Symbol);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateOrder(Order order)
        {
            using (var connection = new SqliteConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "UPDATE Orders SET " +
                                      "ExecutedQuantity = @ExecutedQuantity, Status = @Status " +
                                      "WHERE OrderId = @OrderId";
                command.Parameters.AddWithValue("@ExecutedQuantity", order.ExecutedQuantity);
                command.Parameters.AddWithValue("@Status", order.Status);
                command.Parameters.AddWithValue("@OrderId", order.OrderId);

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Orders";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order
                            {
                                OrderId = reader.GetString(0),
                                Account = reader.GetString(1),
                                ClOrdId = reader.GetString(2),
                                Quantity = reader.GetDecimal(3),
                                ExecutedQuantity = reader.GetDecimal(4),
                                Price = reader.GetDecimal(5),
                                Status = reader.GetInt32(6),
                                Symbol = reader.GetString(7)
                            };

                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }

        public Order GetOrderByClOrdId(string clOrdID)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Orders WHERE ClOrdId = @ClOrdId";
                    command.Parameters.AddWithValue("@ClOrdId", clOrdID);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Order order = new Order
                            {
                                OrderId = reader.GetString(0),
                                Account = reader.GetString(1),
                                ClOrdId = reader.GetString(2),
                                Quantity = reader.GetDecimal(3),
                                ExecutedQuantity = reader.GetDecimal(4),
                                Price = reader.GetDecimal(5),
                                Status = reader.GetInt32(6),
                                Symbol = reader.GetString(7)
                            };

                            return order;
                        }
                    }
                }
            }

            return null;
        }
    }
}
