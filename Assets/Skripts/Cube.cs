using UnityEngine;
using UnityEngine.Pool;

public class Cube : MonoBehaviour
{
    [SerializeField] private float _runTimeMin = 2f;
    [SerializeField] private float _runTimeMax = 5f;

    private float _runTime;
    private ObjectPool<GameObject> _parentPool;
    private bool _isTouchedPlatform = false;

    public void InitializePool(ObjectPool<GameObject> parentPool)
    {
        _parentPool = parentPool;
        _isTouchedPlatform = false;
        GetComponent<Renderer>().material.color = Color.white;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out TouchedPlatform _) && _isTouchedPlatform == false)
        {
            _isTouchedPlatform = true;
            ColorChange();
            Invoke(nameof(DisableCube), SetRandomValye());
        }
    }

    private void ColorChange()
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }

    private void DisableCube()
    {
        _parentPool.Release(gameObject);
    }

    private float SetRandomValye()
    {
        _runTime = Random.Range(_runTimeMin, _runTimeMax);
        return _runTime;
    }
}
