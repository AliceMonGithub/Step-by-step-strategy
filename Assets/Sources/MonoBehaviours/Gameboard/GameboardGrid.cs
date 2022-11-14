using Scripts.Factories;
using Sources.GameboardLogic.CellLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GameboardLogic
{
    public class GameboardGrid : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private Vector2Int _size;
        [SerializeField] private float _spacing;

        [Header("Prefabs")]
        [SerializeField] private CellView _prefab;

        [Header("Components")]
        [SerializeField] private Transform _root;

        [SerializeField] private List<CellConfig> _configs = new();

        private GameboardFactory _factory = new();

        public IReadOnlyCollection<CellConfig> Configs => _configs;

        [ContextMenu("Create")]
        public void Create()
        {
            Clear();

            _configs = _factory.Create(_prefab, _size, _spacing, _root);
        }

        [ContextMenu("Clear")]
        private void Clear()
        {
            for (int i = 0; i < _configs.Count;)
            {
                DestroyImmediate(_configs[i].View.GameObject);

                _configs.RemoveAt(i);
            }
        }
    }
}