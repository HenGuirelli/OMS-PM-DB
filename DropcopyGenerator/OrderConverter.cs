using OMS;
using QuickFix.Fields;

namespace DropcopyGenerator
{
    internal class OrderConverter
    {
        internal QuickFix.FIX44.ExecutionReport Convert(ExecutionReport firstEr)
        {
            return new QuickFix.FIX44.ExecutionReport
            {
                Account = new Account(firstEr.Account),
                ClOrdID = new ClOrdID(firstEr.ClOrdId),
                Price = new Price(firstEr.Price),
                OrderQty = new OrderQty(firstEr.Quantity),
                OrderID = new OrderID(firstEr.OrderId),
                LastQty = new LastQty(firstEr.LastQty),
                ExecType = new ExecType(firstEr.ExecType[0]),
                OrdStatus = new OrdStatus(firstEr.Status.ToString()[0]),
                CumQty = new CumQty(firstEr.CumQty),
                LeavesQty = new LeavesQty(firstEr.LeavesQuantity),
                NoPartyIDs = new NoPartyIDs(0),
                ExecID = new ExecID(Guid.NewGuid().ToString()),
                Symbol = new Symbol(firstEr.Symbol),
                Side = new Side(firstEr.Side),
                AvgPx = new AvgPx(firstEr.Price)
            };
        }
    }
}