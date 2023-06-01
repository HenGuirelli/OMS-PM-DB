using OMS;

namespace DropcopyGenerator
{
    public class OrderGenerator
    {
        private readonly static Random _random = new();
        private readonly List<string> _symbols = new()
        {
            "PETR3",
            "PETR4",
            "VALE3",
            "BBAS3",
        };

        public ExecutionReport NewOrder()
        {
            return new ExecutionReport
            {
                Account = _random.Next(1111, 99999).ToString(),
                ClOrdId = Guid.NewGuid().ToString(),
                OrderId = _random.Next(1111, 99999).ToString(),
                Price = (decimal)(_random.Next(1000) + _random.NextDouble()),
                Quantity = _random.Next(10_000),
                Status = 0, // new
                Symbol = _symbols[_random.Next(_symbols.Count)],
                Side = _random.Next(10) % 2 == 0 ? '1' : '2',
                LastQty = 0,
                ExecType = "0",
                CumQty = 0,
            };
        }

        public ExecutionReport? NextER(ExecutionReport oldEr)
        {
            ExecutionReport? er = null;

            if (oldEr.LeavesQuantity > 1)
            {
                er = CreateNextER(oldEr, 1);
            }
            else if (oldEr.LeavesQuantity == 1)
            {
                er = CreateNextER(oldEr, 2);
            }

            return er;
        }

        private static ExecutionReport CreateNextER(
            ExecutionReport oldEr,
            int status)
        {
            return new ExecutionReport
            {
                Account = oldEr.Account,
                ClOrdId = oldEr.ClOrdId,
                Price = oldEr.Price,
                Quantity = oldEr.Quantity,
                OrderId = oldEr.OrderId,
                LastQty = 1,
                ExecType = "F",
                Status = status,
                CumQty = oldEr.CumQty + 1,
                Side = oldEr.Side,
                Symbol = oldEr.Symbol,
            };
        }
    }
}
