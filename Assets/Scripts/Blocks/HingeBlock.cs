using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Blocks
{
    public class HingeBlock : Block
    {
        public override BlockType Type
        {
            get
            {
                return BlockType.HingeBlock;
            }
        }

        public override void Attach(Block block)
        {
            throw new NotImplementedException();
        }

        public override void Init(Attachment targetAttachment)
        {
            base.Init(targetAttachment);
        }
    }
}
