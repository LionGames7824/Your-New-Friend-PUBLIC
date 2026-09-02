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
        [SerializeField] private BossMovement movement;
        [SerializeField] private BossAttack attack;
        [SerializeField] private BossHealth health;
        [SerializeField] private BossPhaseManager phaseManager;

        [Header("Timing")]
        [SerializeField] private float introDuration = 1.5f;
        [SerializeField] private float baseOrbitDuration = 3f;

        private float orbitDuration;
        private BossState currentState;
        private float stateTimer;

        private void Awake()
        {
            if (movement == null)
                movement = GetComponent<BossMovement>();

            if (attack == null)
                attack = GetComponent<BossAttack>();

            if (health == null)
                health = GetComponent<BossHealth>();

            if (phaseManager == null)
                phaseManager = GetComponent<BossPhaseManager>();

            orbitDuration = baseOrbitDuration;
        }

        private void OnEnable()
        {
            attack.OnAttackFinished += HandleAttackFinished;
            health.OnDeath += HandleDeath;
            phaseManager.OnPhaseChanged += HandlePhaseChanged;
        }

        private void OnDisable()
        {
            attack.OnAttackFinished -= HandleAttackFinished;
            health.OnDeath -= HandleDeath;
            phaseManager.OnPhaseChanged -= HandlePhaseChanged;
        }

        private void Start()
        {
            ChangeState(BossState.Intro);
        }

        private void Update()
        {
            if (currentState == BossState.Dead)
                return;

            stateTimer += Time.deltaTime;

            switch (currentState)
            {
                case BossState.Intro:
                    if (stateTimer >= introDuration)
                        ChangeState(BossState.Orbit);
                    break;

                case BossState.Orbit:
                    if (stateTimer >= orbitDuration)
                        ChangeState(BossState.Laser);
                    break;
            }
        }

        private void ChangeState(BossState newState)
        {
            currentState = newState;
            stateTimer = 0f;

            switch (currentState)
            {
                case BossState.Intro:
                    break;

                case BossState.Orbit:
                    movement.ResumeMovement();
                    movement.StartOrbit();
                    break;

                case BossState.Laser:
                    movement.StopMovement();
                    attack.StartLaser();
                    break;

                case BossState.Dead:
                    movement.StopMovement();
                    break;
            }
        }

        private void HandleAttackFinished()
        {
            if (currentState == BossState.Laser)
                ChangeState(BossState.Orbit);
        }

        private void HandleDeath()
        {
            ChangeState(BossState.Dead);
        }

        private void HandlePhaseChanged(int phase)
        {
            orbitDuration = Mathf.Max(1f, baseOrbitDuration - (phase - 1) * 0.75f);
        }
    }
}