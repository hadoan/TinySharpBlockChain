using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TinySharpBlockChain
{
    public class BlockChainController : ApiController
    {
        [HttpGet, Route("api/blockchain/mine")]
        public object Mine()
        {
            //We run the proof of work algorithm to get the next proof
            var lastBlock = BlockChain.Instance.LastBlock;
            var lastProof = lastBlock.Proof;
            var proof = BlockChain.Instance.ProofOfWork(lastProof);

            //We must receive a reward for finding the proof
            //The sender is "0" to signify that this node has mined a new coin.
            BlockChain.Instance.NewTransaction("0", Program.NodeIdentifier, 1);

            //Forge the new Block by adding it to the chain
            var block = BlockChain.Instance.NewBlock(proof);

            return new
            {
                Message = "New Block Forged",
                block.Index,
                block.Transactions,
                block.Proof,
                block.PreviousHash
            };
        }

        [HttpPost, Route("api/blockchain/newtransaction")]
        public object NewTransaction(Transaction tran)
        {
            if (string.IsNullOrEmpty(tran.Sender) || string.IsNullOrEmpty(tran.Recipient) || tran.Amount <= 0)
                throw new Exception("Transaciton is not valid");

            var index =   BlockChain.Instance.NewTransaction(tran.Sender, tran.Recipient, tran.Amount);
            return $"Transaction will be added to Block {index}";
        }

        [HttpGet, Route("api/blockchain/fullchain")]
        public object FullChain()
        {
            return new FullChainObject
            {
                Chains = BlockChain.Instance.Chains,
                Length = BlockChain.Instance.Chains.Count
            };
        }
    }

    public class FullChainObject
    {
        public int Length { get; set; } 
        public List<Block> Chains { get; set; }
    }
}
