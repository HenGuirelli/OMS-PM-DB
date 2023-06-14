using OMS;
using PM.Collections;

namespace Common
{
    public class Account
    {
        public virtual string AccountNumber { get; set; }
        public virtual string Name { get; set; }
        public virtual ContaCorrente ContaCorrente { get; set; }
        public PmList<Order> Orders { get; } = new();
        //public List<Order> Orders { get; } = new();
    }
}
