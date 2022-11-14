using Sources.GameboardLogic.CellLogic;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scripts.Factories
{
    public class GameboardFactory : IFactory<CellView, Vector2Int, float, Transform, List<CellConfig>>
    {
        public List<CellConfig> Create(CellView prefab, Vector2Int size, float spacing, Transform root)
        {
            List<CellConfig> cells = new();

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3 position = new(x * spacing, y * spacing);

                    CellView cell = Object.Instantiate(prefab, position + root.position, Quaternion.identity, root);

                    cells.Add(new CellConfig(cell, new Vector2Int(x, y)));
                }
            }

            return cells;
        }
    }
}