using UnityEngine;

namespace Sources.GameboardLogic.CellLogic
{
    public class CellView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject _gameObject;

        public GameObject GameObject => _gameObject;
    }
}