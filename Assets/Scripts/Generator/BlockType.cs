using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class BlockType
    {
        public string Name { get; set; }
        public bool IsTransparent { get; set; }
        public bool IsTranslucent { get; set; }
        public bool IsLiquid { get; set; }
        public bool EverySideSame { get; set; }
        public Vector2[] TopUV { get; set; }
        public Vector2[] SideUV { get; set; }
        public Vector2[] BottomUV { get; set; }

        public Vector2[] GetUv (BlockSideEnum side)
        {
            if (side != BlockSideEnum.TOP && side != BlockSideEnum.BOTTOM)
                return this.SideUV;

            if (side == BlockSideEnum.TOP)
                return this.TopUV;

            return this.BottomUV;

        }

    }
}
