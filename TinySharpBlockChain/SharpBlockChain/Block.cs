using System;
using System.Collections.Generic;
using System.Text;

namespace TinySharpBlockChain
{
    public class Block
    {
        public int Index { get; set; }
        public long Timestamp { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int Proof { get; set; }
        public string PreviousHash { get; set; }
    }
}
