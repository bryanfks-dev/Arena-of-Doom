using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class Wave
{
    public int EnemiesCount;
    public int EnemiesLeft;
    public float SpawnRate;
    public float TimeToNextWave;
}

public class GameManager : MonoBehaviour
{
    public GameObject Player;

    public int StartingMonstersCount;
    private int _TotalMultiplier = 0;
    public int MonsterCountMultiplier;

    private List<Wave> _Waves = new List<Wave>();
    private int CurrentWaveIndex = 0;

    private float _WaveCountdown = 0f;
    public float TimeBetweenWave = 5f;

    public GameObject MonstersParent;
    public GameObject[] Monsters;

    // Positions
    public float XRadius;
    public float ZRadius;

    private float _CenterX;
    private float _CenterZ;

    public Transform InventorySlots;
    public Transform MagazineSpawners;

    public GameObject[] MagazineSpawnerPrefabs;

    public static bool IsReady = false;

    // Start is called before the first frame update
    void Start()
    {
        _CenterX = transform.position.x;
        _CenterZ = transform.position.z;

        InitiateNewWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsReady)
        {
            _WaveCountdown -= Time.deltaTime;

            if (_WaveCountdown <= 0f)
            {
                _WaveCountdown = _Waves[CurrentWaveIndex].TimeToNextWave;
                StartCoroutine(SpawnWave());
                InitiateNewWave();
            }
        }
    }

    private void InitializeMagazineSpawner()
    {
        for (int i = 0; i < MagazineSpawners.childCount; i++)
        {
            Transform Child = MagazineSpawners.GetChild(i);

            if (Child.childCount > 0)
            {
                Destroy(Child.GetChild(0).gameObject);
            }

            GameObject Spawner = Instantiate(RandomSpawner(), Child.position, Quaternion.identity);

            Spawner.transform.parent = Child;
        }
    }

    private void InitiateNewWave()
    {
        Wave NewWave = new Wave();
        NewWave.EnemiesCount = StartingMonstersCount + _TotalMultiplier;
        NewWave.SpawnRate = 1f;
        NewWave.TimeToNextWave = TimeBetweenWave;

        _TotalMultiplier += MonsterCountMultiplier;

        _Waves.Add(NewWave);

        InitializeMagazineSpawner();
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < _Waves[CurrentWaveIndex].EnemiesCount; i++)
        {
            Transform _RandomEnemy = RandomEnemy();

            SpawnEnemy(_RandomEnemy);

            yield return new WaitForSeconds(_Waves[CurrentWaveIndex].SpawnRate);
        }

        CurrentWaveIndex++;

        yield break;
    }

    private void SpawnEnemy(Transform Enemy)
    {
        // Set target to current player
        Enemy.GetComponent<Enemy>().TargetPlayer = Player;

        Enemy.SetParent(MonstersParent.transform);
    }

    private Transform RandomEnemy()
    {
        System.Random _Random = new System.Random();

        int RandomIndex = _Random.Next(0, Monsters.Length);

        // Get random positions
        float XPos = UnityEngine.Random.Range(_CenterX - XRadius, _CenterX + XRadius);
        float ZPos = UnityEngine.Random.Range(_CenterZ - ZRadius, _CenterZ + ZRadius);

        NavMeshHit ClosestHit;

        if (NavMesh.SamplePosition(new Vector3(XPos, 0, ZPos), out ClosestHit, 500, 1))
        {
            return Instantiate(Monsters[RandomIndex], ClosestHit.position, Quaternion.identity).transform;
        }

        return Instantiate(Monsters[RandomIndex], new Vector3(XPos, MonstersParent.transform.position.y, ZPos), 
            Quaternion.identity).transform;
    }

    private GameObject RandomSpawner()
    {
        System.Random _Random = new System.Random();
        int RandomIndex = _Random.Next(0, MagazineSpawnerPrefabs.Length);

        return MagazineSpawnerPrefabs[RandomIndex].gameObject;
    }

    public void ResetArena()
    {
        _WaveCountdown = 1f;

        // Destory monsters
        for (int i = 0; i < MonstersParent.transform.childCount; i++)
        {
            Destroy(MonstersParent.transform.GetChild(i).gameObject);
        }

        // Destroy magazine spawners
        for (int i = 0; i < MagazineSpawners.childCount; i++)
        {
            Transform Child = MagazineSpawners.GetChild(i);

            if (Child.GetChild(0) != null)
            {
                Destroy(Child.GetChild(0).gameObject);
            }
        }
    }
}