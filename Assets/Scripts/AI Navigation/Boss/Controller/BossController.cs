using UnityEngine;

namespace YourNewFriend
{
    public enum BossState
    {
        Intro,
        Approach,
        Orbit,
        Laser,
        Dash,
        Dead
    }

    public class BossController : MonoBehaviour
    {
        [SerializeField] private BossMovement _movement;
        [SerializeField] private BossAttack _attack;
        [SerializeField] private BossHealth _health;
        [SerializeField] private BossPhaseManager _phaseManager;

        [Header("Timing")]
        [SerializeField] private float _introDuration = 1.5f;
        [SerializeField] private float _baseOrbitDuration = 3f;

        private float _orbitDuration;
        private BossState _currentState;
        private float _stateTimer;

        private void Awake()
        {
            if (_movement == null)
                _movement = GetComponent<BossMovement>();

            if (_attack == null)
                _attack = GetComponent<BossAttack>();

            if (_health == null)
                _health = GetComponent<BossHealth>();

            if (_phaseManager == null)
                _phaseManager = GetComponent<BossPhaseManager>();

            _orbitDuration = _baseOrbitDuration;
        }

        private void OnEnable()
        {
            _attack.OnAttackFinished += HandleAttackFinished;
            _health.OnDeath += HandleDeath;
            _phaseManager.OnPhaseChanged += HandlePhaseChanged;
        }

        private void OnDisable()
        {
            _attack.OnAttackFinished -= HandleAttackFinished;
            _health.OnDeath -= HandleDeath;
            _phaseManager.OnPhaseChanged -= HandlePhaseChanged;
        }

        private void Start()
        {
            ChangeState(BossState.Intro);
        }

        private void Update()
        {
            if (_currentState == BossState.Dead)
                return;

            _stateTimer += Time.deltaTime;

            switch (_currentState)
            {
                case BossState.Intro:
                    if (_stateTimer >= _introDuration)
                        ChangeState(BossState.Orbit);
                    break;

                case BossState.Orbit:
                    if (_stateTimer >= _orbitDuration)
                        ChangeState(BossState.Laser);
                    break;
            }
        }

        private void ChangeState(BossState newState)
        {
            _currentState = newState;
            _stateTimer = 0f;

            switch (_currentState)
            {
                case BossState.Intro:
                    break;

                case BossState.Orbit:
                    _movement.ResumeMovement();
                    _movement.StartOrbit();
                    break;

                case BossState.Laser:
                    _movement.StopMovement();
                    _attack.StartLaser();
                    break;

                case BossState.Dead:
                    _movement.StopMovement();
                    break;
            }
        }

        private void HandleAttackFinished()
        {
            if (_currentState == BossState.Laser)
                ChangeState(BossState.Orbit);
        }

        private void HandleDeath()
        {
            ChangeState(BossState.Dead);
        }

        private void HandlePhaseChanged(int phase)
        {
            _orbitDuration = Mathf.Max(1f, _baseOrbitDuration - (phase - 1) * 0.75f);
        }
    }
}