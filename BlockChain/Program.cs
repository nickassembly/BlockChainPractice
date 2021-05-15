using System;

namespace BlockChain
{
    class Program
    {
        public interface IBlock
        {
            byte[] Data { get; }
            byte[] Hash { get; set; }
            int Nonce { get; set; }
            byte[] PrevHash { get; set; }
            DateTime TimeStamp { get; }
        }



        static void Main(string[] args)
        {
            
        }
    }
}
