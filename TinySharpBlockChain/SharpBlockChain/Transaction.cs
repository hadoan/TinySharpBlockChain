using System;
using System.Collections.Generic;
using System.Text;

namespace TinySharpBlockChain
{
    public class Transaction
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public double Amount { get; set; }

    }
}
