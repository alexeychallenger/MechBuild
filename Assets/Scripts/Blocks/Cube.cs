using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Blocks
{
    public class Cube : Block
    {
        public override BlockType Type { get { return BlockType.Cube; } }

        public override void Attach(Block block)
        {
            ConnectBlock(block);
        }
    }
}
