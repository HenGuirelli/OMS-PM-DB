namespace OMS
{
    public class ExecutionReport
    {
        public virtual string OrderId { get; set; }
        public virtual string Account { get; set; }
        public virtual string ClOrdId { get; set; }
        public virtual decimal Quantity { get; set; }
        public virtual decimal LastQty { get; set; }
        public virtual decimal CumQty { get; set; }
        public virtual decimal Price { get; set; }
        public virtual decimal LeavesQuantity => Quantity - CumQty;
        public virtual string ExecType { get; set; } = "pending new";
        public string Status { get; set; }
        public string Symbol { get; set; }
        public char Side { get; set; } = '1';
    }
}
