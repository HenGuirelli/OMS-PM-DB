using DropcopyGenerator;
using OMS;

namespace TestProject
{
    public class OrderGeneratorTest
    {
        [Test]
        public void OnGenerator_ShouldFullFilled()
        {
            var orderGenerator = new OrderGenerator(8_000, 1_000);
            var firstExecution = orderGenerator.NewOrder();

            ExecutionReport lastEr = null!;
            foreach (var _ in Enumerable.Range(0, (int)firstExecution.Quantity))
            {
                lastEr = orderGenerator.NextER(lastEr ?? firstExecution);
            }

            Assert.AreEqual(firstExecution.Quantity, lastEr.CumQty);
            Assert.AreEqual(0, lastEr.LeavesQuantity);
        }
    }
}