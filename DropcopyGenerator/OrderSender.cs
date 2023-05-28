using QuickFix;

namespace DropcopyGenerator
{
    public class OrderSender
    {
        private readonly OrderConverter _converter = new();
        private readonly OrderGenerator _orderGenerator = new();

        public void Start(Session session)
        {
            var firstEr = _orderGenerator.NewOrder();
            var firstErConverterd = _converter.Convert(firstEr);
            session.Send(firstErConverterd);

            OMS.ExecutionReport er = _orderGenerator.NextER(firstEr);
            while (er != null)
            {
                var erConverted = _converter.Convert(er);
                session.Send(erConverted);
                er = _orderGenerator.NextER(er);
            }
        }
    }
}
