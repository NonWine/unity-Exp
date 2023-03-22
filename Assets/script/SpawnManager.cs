using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Pool;
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject zombie;
    [SerializeField] private float timeDelay;
    [SerializeField] private float reduceDelay;
    [SerializeField] private float timeLimitDelay;
    [SerializeField] private int setTestWave;
    private ObjectPool<Character> pool;
    private int waveId;
    private int generalCounts;
    private int spawnIndex;
    private int indexAfterBoss;

    private void Awake()
    {
        Instance = this;
        timeDelay = PlayerPrefs.GetFloat("timeDelay", timeDelay);
         waveId = PlayerPrefs.GetInt("waveId", 0); // start LEVEL\Get Current LEVEL
        indexAfterBoss = PlayerPrefs.GetInt("indexAfterBoss", 0);

    }

    private void Start()
    {
        pool = new ObjectPool<Character>(CreateFuncPool, GetFunc, ReasleFunc);

        Spawn();
    }

    public void Spawn(float time =0.1f)
    {
        StartCoroutine(SpawnCor(time));

    }

    private Character CreateFuncPool()
    {
      var mob =  Instantiate(zombie);
        mob.GetComponent<Character>().SetPool(pool);
        return mob.GetComponent<Character>();
    }

    private void GetFunc(Character obj)
    {
        obj.ResetMob();
    }

    private void ReasleFunc(Character obj)
    {
        obj.gameObject.SetActive(false);
    }

    private IEnumerator SpawnCor(float spawnCd)
    {
        yield return new WaitForSeconds(spawnCd);
        for (int i = 0; i < spawnPoints.Length; i++)
        {
          var mob =  pool.Get();
            mob.transform.position = spawnPoints[i].position;
            generalCounts++;
        }
    }

    public void AddDeath()
    {
        generalCounts--;
        if(generalCounts == 0)
        {
            Spawn(3f);
        }
    }

  
}

[System.Serializable]
public class Wave 
{
    public GameObject[] Mob;
    public int[] Count;
}