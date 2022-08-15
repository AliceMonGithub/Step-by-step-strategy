using Scripts.StateMachines;
using Scripts.UnitLogic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.CellLogic
{
    public class Cell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event StateHandler StateChanged;
        public event PointerStateHandler PointerChanged;

        public event PointerHandler PointerClick;

        public Action<Cell> ClickMoving;

        public delegate void StateHandler(IState state, IState oldState, Cell sender);

        public delegate void PointerStateHandler(bool pointerEnter, Cell sender);
        public delegate void PointerHandler(PointerEventData eventData);

        [Header("Data")]
        [SerializeField] private Vector2Int _position;

        [SerializeField] private Unit _unit;
        [SerializeField] private bool _haveUnit;

        [Header("Components")]
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Transform _transform;

        private StateMachine _stateMachine;

        private DefaultState _defaultState;
        private SelectState _selectState;
        private MovingState _movingState;
        private AttackState _attackState;

        private bool _pointerEnter;

        public Vector2Int Position => _position;

        public Unit Unit => _unit;

        public GameObject GameObject => _gameObject;
        public Transform Transform => _transform;

        public IState CurrentState => _stateMachine.CurrentState;

        public DefaultState DefaultState => _defaultState;
        public SelectState SelectState => _selectState;

        public bool HaveUnit => _haveUnit;
        public bool PointerEnter => _pointerEnter;

        private void Awake()
        {
            _stateMachine = new();

            _stateMachine.OnStateChanged += (state, oldState) => StateChanged?.Invoke(state, oldState, this);

            _defaultState = new(_stateMachine, this);
            _selectState = new(_stateMachine, this);
            _movingState = new(_stateMachine, this);
            _attackState = new(_stateMachine, this);

            _stateMachine.Initialze(_defaultState);
        }


        private void OnValidate() // УБРАТЬ ВСЕ ON VALIDATE
        {
            if(_gameObject == null)
            {
                _gameObject = gameObject;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PointerClick?.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _pointerEnter = true;

            PointerChanged?.Invoke(_pointerEnter, this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerEnter = false;

            PointerChanged?.Invoke(_pointerEnter, this);
        }

        public void SetUnit(Unit unit = null)
        {
            if(_unit != null && unit != null)
            {
                Destroy(_unit.GameObject);
            }

            if(unit == null)
            {
                _unit = null;

                _haveUnit = false;
            }
            else
            {
                unit.Transform.position = _transform.position;

                _unit = unit;

                _haveUnit = true;
            }
        }
        public void SetDefault()
        {
            _stateMachine.ChangeState(_defaultState);
        }

        public void SetMoving()
        {
            _stateMachine.ChangeState(_movingState);
        }

        public void SetAttack()
        {
            _stateMachine.ChangeState(_attackState);
        }

        public void Unselect()
        {
            if(CurrentState is SelectState)
            {
                _stateMachine.ChangeState(_defaultState);
            }
        }

        public void Initialize(Vector2Int position)
        {
            _position = position;

            _gameObject.name = $"X: {position.x}, Y: {position.y}";
        }
    }
}
