using UnityEngine;
using UnityEngine.AI;
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    [SerializeField] private MobStats _playerStats;
    [SerializeField] private Detector detector;
    [SerializeField] private CameraFollowing cameraFollow;
    [SerializeField] private Animator _playerAnim;
    private Weapon weapon;
    private HealthUI playerUI;
    private bool dead;
    private int hpLeft;
    private bool setDefaultKolchan;
    private float maxD;
    private void Awake()
    {
        Instance = this;
        setDefaultKolchan =  PlayerPrefsExtra.GetBool("kolchan", false);
    }

    private void Start()
    {
        maxD = Mathf.Infinity;
        weapon = GetComponent<Weapon>();
        playerUI = GetComponent<HealthUI>();
        _playerStats.SetDefaltPlayer();
        playerUI.SetHealth(_playerStats.GetHealth());
        hpLeft = PlayerPrefs.GetInt("hpLeft",0);
        playerUI.GetDamageUI(hpLeft);
        _playerStats.ReduceHealth(hpLeft);
        cameraFollow.SetPlayer(transform);
        Atc();
    }

    private void Update()
    {
        RaycastHit hit;

        Vector3 p1 = transform.position;

        // Cast a sphere wrapping character controller 10 meters forward
        // to see if it is about to hit anything.
        Collider[] colls = Physics.OverlapSphere(transform.position, 5);
        GameObject closestObj = this.gameObject;
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].CompareTag("Enemy"))
            {
                Vector3 dir = colls[i].transform.position - transform.position;
                if(maxD >= dir.magnitude)
                {
                    maxD = dir.magnitude;
                    closestObj = colls[i].gameObject;
                }
                
            }
        }
        //Debug.Log(closestObj.name + "Closest Object");
    }

    private void Atc()
    {
       
    }

    private void Death()
    {

        cameraFollow.enabled = false;
        dead = true;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
        detector.enabled = false;
        playerUI.TurnOffUiHP();
        _playerAnim.SetLayerWeight(0, 1f);
        _playerAnim.SetLayerWeight(1, 0f);
        _playerAnim.SetBool("Death", true);
        
        Invoke(nameof(GameLose), 2f);
    }

    private void GameLose()
    {
       // GameManager.Instance.GameLose();
        Destroy(gameObject);
    }

    public MobStats GetPlayerStats() { return _playerStats; }

    public void GetDamage(int dmg)
    {
     //   ParticlePool.Instance.PlayBlood(transform.position);
        _playerStats.ReduceHealth(dmg);
        hpLeft = 200 - _playerStats.GetHealth();
        PlayerPrefs.SetInt("hpLeft", hpLeft);
        playerUI.GetDamageUI((float)dmg);
        if (_playerStats.GetHealth() <= 0)
            Death();
    }
    
    public void HealPlayer()
    {
        playerUI.ResetUIHealth();
        _playerStats.SetHealth(playerUI.Health());
      
    }

    public void Respawn()
    {
        transform.position = new Vector3(0f, 0f, -12f);
        gameObject.SetActive(true);
        _playerAnim.SetBool("Death", false);
        cameraFollow.enabled = true;
        dead = false;
        detector.enabled = true;
        HealPlayer();
    }


    public int GetHealth() { return _playerStats.GetHealth(); }

    public bool isDead() { return dead; }

    

}