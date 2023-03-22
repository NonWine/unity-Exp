using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
public class Character : MonoBehaviour
{
  
    [SerializeField] private int _rewardMoney;
    [SerializeField] private int _distance;
    [SerializeField] private Animator myAnim;
    [SerializeField] private Detector _detector;
    [SerializeField] private Coin coin;
    [SerializeField] private SkinnedMeshRenderer mesh;
    [SerializeField] protected MobStats stats;
    private NavMeshAgent myAgent;
    private HealthUI healthUI;
    private Transform target;
    private bool dead, attaking;
    private float timer;
    private IObjectPool<Character> pool;
    private void Start()
    {
        SetStats();
        healthUI = gameObject.GetComponent<HealthUI>();
        myAgent = GetComponent<NavMeshAgent>();
        healthUI.SetHealth(stats.GetHealth());
        myAgent.speed = stats.GetSpeed();
    }

    private void Update()
    {
        if (!isDead())
        {
            if (healthUI != null)
                healthUI.GetCanvas().GetComponent<RectTransform>().rotation = Quaternion.Euler(-90f, transform.rotation.y, 0f);
            if (target != null)
            {
                Move();
            }
            FindClosetsTarget();
        }
    }

    private void FindClosetsTarget()
    {
        _detector.setPointInfinity();
        _detector.TryFindTheNearlestEnemy(transform.position);
        if (_detector.isDetected())
            target = _detector.GetTargetInfo();

    }

    private void Rotation(Transform target)
    {
        Vector3 dir = target.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 14 * Time.deltaTime);

    }

    private void SpawnCoins()
    {
        for (int i = 0; i < _rewardMoney; i++)
        {
            //  NightPool.Spawn(coin, transform.position + (Vector3.up * 6f), Quaternion.Euler(0f,0f,90f));
        }
    }

    protected virtual void Move()
    {
        if (Vector3.Distance(transform.position, target.position) <= _distance)
            attaking = true;

        if (attaking == false)
        {
            myAnim.SetInteger("state", 1);
            myAgent.isStopped = false;
        }
        else if (attaking)
        {
            myAgent.isStopped = true;
            myAgent.velocity = Vector3.zero;
            GiveDamage();
        }
        myAgent.SetDestination(target.position);
    }

    protected virtual void GiveDamage()
    {
        myAnim.SetInteger("state", 2);
        Rotation(target);
        if (target.CompareTag("Player"))
            GiveDamagePlayer(Player.Instance);
    }

    protected virtual void GiveDamagePlayer(Player player)
    {
        if (!player.isDead() && !isDead() && attaking)
        {
            timer += Time.deltaTime;

            if (timer >= stats.GetCD())
            {
                player.GetDamage(stats.GetDamage());
                timer = 0f;
                attaking = false;
            }

        }
        else
            myAnim.SetInteger("state", 0);
    }

    protected virtual void SetStats()
    {
            stats.SetDefaultZombie();
    }

    public virtual void GetDamage(int dmg, int arrowdmg = 0)
    {
        if (!attaking)
            myAnim.SetTrigger("takeDamage");
        stats.ReduceHealth(dmg);
        healthUI.GetDamageUI(dmg);
        if (healthUI.GetfillAmout() <= 0 && !dead)
            Death();
    }

    protected virtual void Death()
    {
        myAgent.isStopped = true;
        myAgent.velocity = Vector3.zero;
        dead = true;
        healthUI.TurnOffUiHP();
        SpawnManager.Instance.AddDeath();
        SpawnCoins();
        pool.Release(this);
    }

    public bool isDead() { return dead; }

    public void ResetMob()
    {
        if (dead)
        {
            gameObject.SetActive(true);
            stats.SetDefaultZombie();
            Debug.Log(stats.GetHealth());
            healthUI.ResetUIHealth();
            healthUI.SetHealth(stats.GetHealth());
            myAgent.isStopped = false;
            dead = false;
        }

    }

    public void SetPool(IObjectPool<Character> pool) => this.pool = pool; 

}
