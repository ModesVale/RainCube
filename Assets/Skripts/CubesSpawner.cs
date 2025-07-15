using UnityEngine;
using UnityEngine.Pool;

public class CubesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Vector3 _spawnAreaSize = new Vector3(10f, 2f, 10f);
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;
    [SerializeField] private Transform _spawnCenter;

    private ObjectPool<GameObject> _pool; 
    


    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (prefub) => ActionOnGet(prefub),
            actionOnRelease: (prefub) => prefub.SetActive(false),
            actionOnDestroy: (prefub) => Destroy(prefub),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void ActionOnGet(GameObject prefub)
    {
        prefub.transform.position = GetRandomSpawnPoint();
        prefub.GetComponent<Rigidbody>().velocity = Vector3.zero;
        prefub.GetComponent<Cube>().InitializePool(_pool);
        prefub.SetActive(true);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private Vector3 GetRandomSpawnPoint()
    {
        float halfDivisior = 0.5f;

        float x = Random.Range(-_spawnAreaSize.x * halfDivisior, _spawnAreaSize.x * halfDivisior);
        float y = Random.Range(-_spawnAreaSize.y * halfDivisior, _spawnAreaSize.y * halfDivisior);
        float z = Random.Range(-_spawnAreaSize.z * halfDivisior, _spawnAreaSize.z * halfDivisior);

        return _spawnCenter.position + new Vector3(x, y, z);
    }
}
