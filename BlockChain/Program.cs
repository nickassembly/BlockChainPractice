using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using static BlockChain.Program;

namespace BlockChain
{
    public static class BlockChainExtension
    {
        public static byte[] GenerateHash(this IBlock block)
        {
            using (SHA512 sha = new SHA512Managed())
            using (MemoryStream st = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(st))
            {
                bw.Write(block.Data);
                bw.Write(block.Nonce);
                bw.Write(block.TimeStamp.ToBinary());
                bw.Write(block.PrevHash);
                var starr = st.ToArray();
                return sha.ComputeHash(starr);
            }
        }
        public static byte[] MineHash(this IBlock block, byte[] difficulty) { }
        public static bool IsValid(this IBlock block) { }
        public static bool IsValidPrevBlock(this IBlock block, IBlock prevBlock) { }
        public static bool IsValid(this IEnumerable<IBlock> items) { }
    }

    public interface IBlock
    {
        byte[] Data { get; }
        byte[] Hash { get; set; }
        int Nonce { get; set; }
        byte[] PrevHash { get; set; }
        DateTime TimeStamp { get; }
    }

    class Program
    {
       

        public class Block : IBlock
        {
            public byte[] Data { get; }

            public byte[] Hash { get; set; }
            public int Nonce { get; set; }
            public byte[] PrevHash { get; set; }
            public DateTime TimeStamp { get; }

            public override string ToString()
            {
                return $"{BitConverter.ToString(Hash).Replace("-", "")}:\n{BitConverter.ToString(PrevHash).Replace("-", "")}\n {Nonce} {TimeStamp}";
            }
        }

        public class BlockChain : IEnumerable<IBlock>
        {
            private List<IBlock> _items = new List<IBlock>();

            public BlockChain(byte[] difficulty, IBlock genesis)
            {
                Difficulty = difficulty;
                genesis.Hash = genesis.MineHash(difficulty);
                Items.Add(genesis);
            }

            public void Add(IBlock item)
            {
                if (Items.LastOrDefault() != null)
                {
                    item.PrevHash = Items.LastOrDefault()?.Hash;
                }
                item.Hash = item.MineHash(Difficulty);
                Items.Add(item);
            }

            public int Count => Items.Count;
            public IBlock this[int index]
            {
                get => Items[index];
                set => Items[index] = value;
            }

            public byte[] Difficulty { get;  }

            public List<IBlock> Items
            {
                get => _items;
                set => _items = value;
            }

            public IEnumerator<IBlock> GetEnumerator()
            {
                return Items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return Items.GetEnumerator();
            }
        }

        static void Main(string[] args)
        {
            
        }
    }
}
