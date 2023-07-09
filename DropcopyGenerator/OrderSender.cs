using QuickFix;

namespace DropcopyGenerator
{
    public class OrderSender
    {
        private readonly OrderConverter _converter = new();
        private readonly OrderGenerator _orderGenerator = new(8_000, 1_000);

        public bool Start(Session session)
        {
            var firstEr = _orderGenerator.NewOrder();
            var firstErConverterd = _converter.Convert(firstEr);
            session.Send(firstErConverterd);

            OMS.ExecutionReport? er = _orderGenerator.NextER(firstEr);
            while (er != null)
            {
                var erConverted = _converter.Convert(er);
                if (!session.IsLoggedOn)
                {
                    Console.WriteLine("Sessao desconectada");
                    return false;
                }
                session.Send(erConverted);
                er = _orderGenerator.NextER(er);
            }
            return true;
        }
    }
}
