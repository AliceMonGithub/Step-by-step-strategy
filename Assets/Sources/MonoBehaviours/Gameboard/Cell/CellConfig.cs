using System;
using UnityEngine;

namespace Sources.GameboardLogic.CellLogic
{
    [Serializable]
    public class CellConfig
    {
        public readonly CellView View;
        public readonly Vector2Int Position;

        public CellConfig(CellView view, Vector2Int position)
        {
            View = view;
            Position = position;
        }
    }
}