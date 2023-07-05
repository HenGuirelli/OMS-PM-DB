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
        private readonly List<string> _accounts = new()
        {
            "8834",
"1683",
"2533",
"9605",
"4415",
"2738",
"1376",
"9013",
"3832",
"4692",
"8983",
"7798",
"7002",
"4611",
"5626",
"5850",
"1589",
"8400",
"8964",
"2168",
"2816",
"1619",
"7552",
"5857",
"3066",
"8343",
"2731",
"5208",
"9427",
"6067",
"8660",
"7057",
"8091",
"3004",
"2119",
"2667",
"5831",
"1548",
"5365",
"3052",
"6632",
"7469",
"4420",
"9805",
"3110",
"4431",
"8285",
"1817",
"3889",
"5376",
"1442",
"7998",
"2201",
"7824",
"2648",
"4294",
"5054",
"1026",
"2473",
"3129",
"4116",
"1530",
"2828",
"1881",
"9146",
"3031",
"5497",
"5521",
"8638",
"8741",
"8461",
"8797",
"7183",
"6259",
"6362",
"9003",
"7405",
"1874",
"7872",
"1940",
"3335",
"5745",
"8715",
"8448",
"4422",
"2846",
"3036",
"5549",
"1889",
"5171",
"3564",
"6440",
"3824",
"3741",
"1421",
"6124",
"9989",
"5438",
"3925",
"2396"
        };

        public ExecutionReport NewOrder()
        {
            return new ExecutionReport
            {
                Account = _accounts[_random.Next(0, _accounts.Count)],
                ClOrdId = Guid.NewGuid().ToString(),
                OrderId = _random.Next(1111, 99999).ToString(),
                Price = (decimal)(_random.Next(1000) + _random.NextDouble()),
                Quantity = 6_000, //_random.Next(10_000),
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
                LastQty = 1000,
                CumQty = oldEr.CumQty + 1000,
                ExecType = "F",
                Status = status,
                Side = oldEr.Side,
                Symbol = oldEr.Symbol,
            };
        }
    }
}
