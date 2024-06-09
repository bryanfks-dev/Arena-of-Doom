using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Atributes")]
    public float HealthPoint;
    public float AtkDmg;

    public GameObject TargetPlayer;
    private NavMeshAgent _Agent;
    private Animator _Animator;
    public float MovementSpd;

    public LayerMask PlayerMask;

    // Attacking
    private bool _PlayerInAttackRange;
    public float TimeBetweenAttacks;
    private bool _IsAttacking;
    public float AttackRange;

    // Start is called before the first frame update
    void Start()
    {
        // Get current object NavMesgAgent components
        _Agent = GetComponent<NavMeshAgent>();
        _Animator = GetComponent<Animator>();

        // Set agent movement speed
        _Agent.speed = MovementSpd;
    }

    // Update is called once per frame
    void Update()
    {
        _PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, PlayerMask);

        if (_PlayerInAttackRange)
        {
            AttackPlayer();
        }
        else if (!_PlayerInAttackRange && !_IsAttacking)
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        _Agent.SetDestination(TargetPlayer.transform.position);
    }

    private void AttackPlayer()
    {
        _Agent.SetDestination(transform.position);

        transform.LookAt(new Vector3(
            TargetPlayer.transform.position.x, transform.position.y, TargetPlayer.transform.position.z));

        if (!_IsAttacking)
        {
            _IsAttacking = true;

            transform.LookAt(transform.position);

            _Animator.SetTrigger("Attack");

            float AnimationLength = _Animator.GetCurrentAnimatorStateInfo(0).length;

            Invoke(nameof(DecideNextAction), AnimationLength + 1.5f);
        }
    }

    private void DecideNextAction()
    {
        if (Physics.CheckSphere(transform.position, AttackRange, PlayerMask))
        {
            Invoke(nameof(ResetAttack), TimeBetweenAttacks);
        }
        else
        {
            ResetAttack();
        }
    }

    private void ResetAttack()
    {
        _IsAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        HealthPoint -= damage;

        if (HealthPoint <= 0)
        {
            Invoke(nameof(DestroyEnemy), .5f);
        }
    }

    private void DestroyEnemy()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon" || other.gameObject.tag == "projectile")
        {
            float WeaponDamage = other.gameObject.GetComponent<Weapon>().AttackDmg;
            
            TakeDamage(WeaponDamage);
        }
    }
}
