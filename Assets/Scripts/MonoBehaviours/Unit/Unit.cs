using Scripts.CellLogic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Scripts.UnitLogic
{
    public class Unit : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private Vector2Int[] _moves;
        [SerializeField] private Vector2Int[] _attackMoves;

        [Space]

        [SerializeField] private Cell _cell;

        [Header("Gizmos")]
        [SerializeField] private Color _moveColor = Color.white;
        [SerializeField] private Color _attackColor = Color.white;
        [SerializeField] private Color _universalColor = Color.white;

        [Space]

        [SerializeField] private float _spacing;

        [Header("Components")]
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Transform _transform;

        public Vector2Int[] Moves => _moves; // РЕФАКТОРИНГ 
        public Vector2Int[] AttackMoves => _attackMoves;

        public GameObject GameObject => _gameObject;
        public Transform Transform => _transform;

        private void OnDrawGizmosSelected()
        {
            if (_moves == null || _attackMoves == null) return;

            foreach (Vector2 move in _moves)
            {
                Vector2 position = move * _transform.localScale * _spacing + (Vector2)_transform.localPosition;

                if (_attackMoves.Any(attackMove => attackMove == move))
                {
                    Gizmos.color = _universalColor;

                    Gizmos.DrawWireCube(position, _transform.localScale);

                    continue;
                }

                Gizmos.color = _moveColor;

                Gizmos.DrawWireCube(position, _transform.localScale);
            }

            Gizmos.color = _attackColor;

            foreach (Vector2 attackMove in _attackMoves)
            {
                Vector2 position = attackMove * _transform.localScale * _spacing + (Vector2)_transform.localPosition;

                if (_moves.Any(move => move == attackMove)) continue;

                Gizmos.DrawWireCube(position, _transform.localScale);
            }
        }

        public void Initialize(Cell cell)
        {
            _cell.SetUnit();

            _transform.position = cell.Transform.position;

            cell.SetUnit(this);

            _cell = cell;
        }

        [ContextMenu("FindCell")]
        public void FindCell()
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(_transform.position, _transform.localScale, 0);

            if (colliders.Length == 0) return;

            Cell cell = null;
            colliders.First(collider => { cell = collider.GetComponent<Cell>(); return cell != null; });

            if (cell != null)
            {
                cell.SetUnit(this);

                _cell = cell;
            }
        }

        [ContextMenu("Reverse moves")]
        private void ReverseMoves()
        {
            for (int i = 0; i < _moves.Length; i++)
            {
                _moves[i].x *= -1;
                _moves[i].y *= -1;
            }

            for (int i = 0; i < _attackMoves.Length; i++)
            {
                _attackMoves[i].x *= -1;
                _attackMoves[i].y *= -1;
            }

            //EditorUtility.SetDirty(this);

            //AssetDatabase.SaveAssetIfDirty(this);
        }
    }
}