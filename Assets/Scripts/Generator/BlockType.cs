using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts
{
    public class BlockType
    {
        public string Name { get; set; } // Nazwa bloku
        public bool IsTransparent { get; set; } // Czy jest przezroczysta
        public bool IsTranslucent { get; set; } // Czy jest półprzezroczysta
        public bool IsLiquid { get; set; } // Czy jest płynna
        public bool EverySideSame { get; set; } // Czy wszystkie ściany maja taką samą strukturę
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
