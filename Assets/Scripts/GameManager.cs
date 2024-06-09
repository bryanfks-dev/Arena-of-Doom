using MikeNspired.UnityXRHandPoser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Wave
{
    public int EnemiesCount;
    public int EnemiesLeft;
    public float SpawnRate;
    public float TimeToNextWave;
}

public class GameManager : MonoBehaviour
{
    public GameObject OverallPlayer;
    private Transform _PlayerCamera;

    public int StartingMonstersCount;
    private int _TotalMultiplier = 0;
    public int MonsterCountMultiplier;

    private List<Wave> _Waves = new List<Wave>();
    private int CurrentWaveIndex = 0;

    private float _WaveCountdown = 0f;
    public float TimeBetweenWave = 30f;

    public float MagazineSpawnCountdown = 5f;

    public GameObject MonstersParent;
    public GameObject[] Monsters;

    // Positions
    public float XRadius;
    public float ZRadius;

    private float _CenterX;
    private float _CenterZ;

    public Transform InventorySlots;
    public Transform MagazineSpawners;

    private string[] _WeaponNames = new string[2];
    private GameObject[] _WeaponMagazines = {};

    // Start is called before the first frame update
    void Start()
    {
        InitializePlayer();

        GetWeaponNames();
        LoadWeaponMagazines();

        _CenterX = transform.position.x;
        _CenterZ = transform.position.z;

        InitiateNewWave();
    }

    // Update is called once per frame
    void Update()
    {
        _WaveCountdown -= Time.deltaTime;

        if (_WaveCountdown <= 0f)
        {
            Debug.Log(CurrentWaveIndex);
            _WaveCountdown = _Waves[CurrentWaveIndex].TimeToNextWave;
            StartCoroutine(SpawnWave());
            InitiateNewWave();
        }
    }

    private void InitializePlayer()
    {
        OverallPlayer = DontDestroyObjects.Player;

        Debug.Log(OverallPlayer);

        // Change player attributes
        DontDestroyObjects.ArenaSpawn();

        OverallPlayer.SetActive(false);
        OverallPlayer.SetActive(true);

        // Find grandchild
        DynamicMoveProvider XROrigin =
            OverallPlayer.transform.GetChild(0).Find("XR Origin").GetComponent<DynamicMoveProvider>();

        XROrigin.moveSpeed = 45;

        _PlayerCamera = 
            OverallPlayer.transform.GetChild(0).Find("XR Origin").Find("CameraOffset").Find("Main Camera");
    }

    private void GetWeaponNames()
    {
        for (int i = 0; i < InventorySlots.childCount; i++)
        {
            InventorySlot Slot = InventorySlots.GetChild(i).GetComponent<InventorySlot>();

            if (Slot.CurrentSlotItem != null)
            {
                _WeaponNames[i] = Slot.CurrentSlotItem.gameObject.name;
            }
        }
    }

    private void LoadWeaponMagazines()
    {
        Dictionary<string, string> WeaponMagezinePairs = new Dictionary<string, string>()
        {
            { "Rifle 2Hand- Auto Fire", "AR Magazine Model" },
            { "HandGun Reloading Variant", "HandGun Magazine" },
        };

        int WeaponMagazinesIndex = 0;

        for (int i = 0; i < _WeaponNames.Length; i++)
        {
            // Check if weapon is exist, and magazine is available for this weapon
            if (_WeaponNames[i] != null && WeaponMagezinePairs.ContainsKey(_WeaponNames[i]))
            {
                _WeaponMagazines[WeaponMagazinesIndex++] = 
                    Resources.Load("/Weapons/Magazines/" + WeaponMagezinePairs[_WeaponNames[i]]) as GameObject;
            }
        }
    }

    private void InitializeMagazineSpawner()
    {
        if (_WeaponMagazines.Length > 0)
        {
            for (int i = 0; i < MagazineSpawners.childCount; i++)
            {
                Transform Child = MagazineSpawners.GetChild(i);

                ObjectSpawner ObjSpawner = Child.GetComponent<ObjectSpawner>();

                ObjSpawner.Prefab = RandomMagazine();
                ObjSpawner.spawnTimer = MagazineSpawnCountdown;
            }
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

        // InitializeMagazineSpawner();
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < _Waves[CurrentWaveIndex].EnemiesCount; i++)
        {
            Transform _RandomEnemy = RandomEnemy();

            SpawnEnemy(_RandomEnemy);

            yield return new WaitForSeconds(_Waves[CurrentWaveIndex].SpawnRate);
        }

        yield break;
    }

    private void SpawnEnemy(Transform Enemy)
    {
        // Set target to current player
        Enemy.GetComponent<Enemy>().TargetPlayer = _PlayerCamera.gameObject;

        Enemy.SetParent(MonstersParent.transform);
    }

    private Transform RandomEnemy()
    {
        System.Random _Random = new System.Random();

        int RandomIndex = _Random.Next(0, Monsters.Length);

        // Get random positions
        float XPos = UnityEngine.Random.Range(_CenterX - XRadius, _CenterX + XRadius);
        float ZPos = UnityEngine.Random.Range(_CenterZ - ZRadius, _CenterZ + ZRadius);

        return Instantiate(Monsters[RandomIndex], new Vector3(XPos, MonstersParent.transform.position.y, ZPos), 
            Quaternion.identity).transform;
    }

    private GameObject RandomMagazine()
    {
        System.Random _Random = new System.Random();
        int RandomIndex = _Random.Next(0, _WeaponMagazines.Length);

        return _WeaponMagazines[RandomIndex].gameObject;
    }
}