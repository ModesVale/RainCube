using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubesSpawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private Vector3 _spawnAreaSize = new Vector3(10f, 2f, 10f);
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;
    [SerializeField] private Transform _spawnCenter;

    private ObjectPool<Cube> _pool;
    private const float InitialDelay = 0f;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (instance) => ActionOnGet(instance),
            actionOnRelease: OnRelease,
            actionOnDestroy: (instance) => Destroy(instance),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void ActionOnGet(Cube instance)
    {
        instance.RegisterReturnAction(() => _pool.Release(instance));
        instance.transform.position = GetRandomSpawnPoint();
        instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        instance.SetDefaultConfig();
        instance.gameObject.SetActive(true);
    }

    private void OnRelease(Cube instance)
    {
        instance.UnregisterReturnAction();
        instance.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(InitialDelay);

        while (true)
        {
            GetCube();
            yield return new WaitForSeconds(_repeatRate);
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        float halfDivisior = 0.5f;

        float x = Random.Range(-_spawnAreaSize.x * halfDivisior, _spawnAreaSize.x * halfDivisior);
        float y = Random.Range(-_spawnAreaSize.y * halfDivisior, _spawnAreaSize.y * halfDivisior);
        float z = Random.Range(-_spawnAreaSize.z * halfDivisior, _spawnAreaSize.z * halfDivisior);

        return _spawnCenter.position + new Vector3(x, y, z);
    }

    private void OnDrawGizmosSelected()
    {
        if (_spawnCenter == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_spawnCenter.position, _spawnAreaSize);
    }
}
