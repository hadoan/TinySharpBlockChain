﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TinySharpBlockChain
{
    public class BlockChain
    {
        private static volatile BlockChain instance;
        private static object syncRoot = new Object();

        private BlockChain()
        {
            CurrentTransactions = new List<Transaction>();
            Chains = new List<Block>();

            //Create the genesis block
            NewBlock(100, "1");
        }

        public static BlockChain Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new BlockChain();
                    }
                }

                return instance;
            }
        }

        public List<Block> Chains { get; set; }

        public List<Transaction> CurrentTransactions { get; set; }

        /// <summary>
        /// Create a new Block in the Blockchain
        /// </summary>
        /// <param name="proof">The proof given by the Proof of Work algorithm</param>
        /// <param name="previousHash">Hash of previous Block</param>
        public Block NewBlock(int proof, string previousHash=null) {
            var block = new Block
            {
                Index = Chains.Count + 1,
                Timestamp = DateTime.Now.ToUnixTime(),
                Transactions=CurrentTransactions,
                Proof = proof,
                PreviousHash = previousHash??Hash(LastBlock)
            };
            //Reset the current list of transactions
            CurrentTransactions = new List<Transaction>();
            Chains.Add(block);
            return block;
        }

        /// <summary>
        /// Creates a new transaction to go into the next mined Block
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="recipient"></param>
        /// <param name="amout"></param>
        public int NewTransaction(string sender, string recipient, double amount)
        {

            var tran = new Transaction()
            {
                Sender = sender,
                Recipient = recipient,
                Amount = amount
            };
            CurrentTransactions.Add(tran);
            return LastBlock.Index + 1;
        }

        /// <summary>
        /// Simple Proof of Work Algorithm:
        /// - Find a number p' such that hash(pp') contains leading 4 zeroes, where p is the previous p'
        /// - p is the previous proof, and p' is the new proof
        /// </summary>
        /// <param name="lastProof"></param>
        public int ProofOfWork(int lastProof)
        {
            var proof = 0;
            while (!ValidProof(lastProof, proof))
                proof += 1;

            return proof;
        }

        /// <summary>
        /// Validates the Proof: Does hash(last_proof, proof) contain 4 leading zeroes?
        /// </summary>
        /// <param name="lastProof"></param>
        /// <param name="proof"></param>
        private bool ValidProof(int lastProof, int proof)
        {
            var guess = $"{lastProof}{proof}";
            var guessHash = guess.Sha256();
            return guessHash.StartsWith("0000");
        }

        /// <summary>
        ///Creates a SHA-256 hash of a Block
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static string Hash(Block block) {
            return JsonConvert.SerializeObject(block).Sha256();
        }

        public Block LastBlock
        {
            get => Chains[this.Chains.Count - 1];
        }
    }
}
