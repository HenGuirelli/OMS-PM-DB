﻿namespace OMS
{
    public class Order
    {
        public virtual string OrderId { get; set; }
        public virtual string Account { get; set; }
        public virtual string ClOrdId { get; set; }
        public virtual decimal Quantity { get; set; }
        public virtual decimal ExecutedQuantity { get; set; }
        public virtual decimal LeavesQuantity => Quantity - ExecutedQuantity;
        public virtual decimal Price { get; set; }
        public virtual string Status { get; set; }
        public virtual string Symbol { get; set; }

        public void New()
        {
            Status = "NEW";
        }

        public void Filled()
        {
            Status = "FILLED";
        }

        public virtual List<ExecutionReport> ExecutionReports { get; set; }
    }
}
