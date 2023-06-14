using Npgsql;

namespace OMS.Repositories
{
    public class PostgreSqlOrderRepository : IOrderRepository
    {
        private string connectionString;

        public PostgreSqlOrderRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddOrder(Order order)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var tx = connection.BeginTransaction();
                try
                {
                    using (var command = new NpgsqlCommand(
                        "INSERT INTO Orders (OrderId, Account, ClOrdId, Quantity, ExecutedQuantity, Price, Status, Symbol) VALUES (@OrderId, @Account, @ClOrdId, @Quantity, @ExecutedQuantity, @Price, @Status, @Symbol)", connection))
                    {
                        command.Transaction = tx;
                        command.Parameters.AddWithValue("OrderId", order.OrderId);
                        command.Parameters.AddWithValue("Account", order.Account);
                        command.Parameters.AddWithValue("ClOrdId", order.ClOrdId);
                        command.Parameters.AddWithValue("Quantity", order.Quantity);
                        command.Parameters.AddWithValue("ExecutedQuantity", order.ExecutedQuantity);
                        command.Parameters.AddWithValue("Price", order.Price);
                        command.Parameters.AddWithValue("Status", order.Status);
                        command.Parameters.AddWithValue("Symbol", order.Symbol);
                        command.ExecuteNonQuery();
                    }
                    using (var command = new NpgsqlCommand(
                        "UPDATE contacorrente set = value - @Value where account = @Account ", connection))
                    {
                        command.Transaction = tx;
                        command.Parameters.AddWithValue("Account", order.Account);
                        command.Parameters.AddWithValue("Value", order.Price);
                        command.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                }
            }
        }

        public IEnumerable<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM Orders", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order();
                            order.OrderId = reader.GetString(reader.GetOrdinal("OrderId"));
                            order.Account = reader.GetString(reader.GetOrdinal("Account"));
                            order.ClOrdId = reader.GetString(reader.GetOrdinal("ClOrdId"));
                            order.Quantity = reader.GetDecimal(reader.GetOrdinal("Quantity"));
                            order.ExecutedQuantity = reader.GetDecimal(reader.GetOrdinal("ExecutedQuantity"));
                            order.Price = reader.GetDecimal(reader.GetOrdinal("Price"));
                            order.Status = reader.GetInt32(reader.GetOrdinal("Status"));
                            order.Symbol = reader.GetString(reader.GetOrdinal("Symbol"));
                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }

        public Order GetOrderByClOrdId(string clOrdID)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM Orders WHERE ClOrdId = @ClOrdId", connection))
                {
                    command.Parameters.AddWithValue("ClOrdId", clOrdID);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Order order = new Order();
                            order.OrderId = reader.GetString(reader.GetOrdinal("OrderId"));
                            order.Account = reader.GetString(reader.GetOrdinal("Account"));
                            order.ClOrdId = reader.GetString(reader.GetOrdinal("ClOrdId"));
                            order.Quantity = reader.GetDecimal(reader.GetOrdinal("Quantity"));
                            order.ExecutedQuantity = reader.GetDecimal(reader.GetOrdinal("ExecutedQuantity"));
                            order.Price = reader.GetDecimal(reader.GetOrdinal("Price"));
                            order.Status = reader.GetInt32(reader.GetOrdinal("Status"));
                            order.Symbol = reader.GetString(reader.GetOrdinal("Symbol"));
                            return order;
                        }
                    }
                }
            }
            return null;
        }

        public void UpdateOrder(Order order)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("UPDATE Orders SET Account = @Account, Quantity = @Quantity, ExecutedQuantity = @ExecutedQuantity, Price = @Price, Status = @Status, Symbol = @Symbol WHERE ClOrdId = @ClOrdId", connection))
                {
                    command.Parameters.AddWithValue("Account", order.Account);
                    command.Parameters.AddWithValue("Quantity", order.Quantity);
                    command.Parameters.AddWithValue("ExecutedQuantity", order.ExecutedQuantity);
                    command.Parameters.AddWithValue("Price", order.Price);
                    command.Parameters.AddWithValue("Status", order.Status);
                    command.Parameters.AddWithValue("Symbol", order.Symbol);
                    command.Parameters.AddWithValue("ClOrdId", order.ClOrdId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

}
