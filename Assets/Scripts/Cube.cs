using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class Cube : MonoBehaviour
{
    [SerializeField] private float _runTimeMin = 2f;
    [SerializeField] private float _runTimeMax = 5f;

    private float _runTime;
    private ObjectPool<GameObject> _parentPool;
    private bool _isTouchedPlatform = false;
    private UnityAction _releaseAction;
    public event UnityAction CubeTouched;

    public void SetDefaultConfig()
    {
        _isTouchedPlatform = false;
        GetComponent<ColorChanger>().SetDefaultColor();
        transform.rotation = Quaternion.identity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Platform _) && _isTouchedPlatform == false)
        {
            _isTouchedPlatform = true;
            GetComponent<ColorChanger>().SetRandomColor();
            StartCoroutine(CooldownDisable());
        }
    }

    public void RegisterReturnAction(UnityAction returnAction)
    {
        _releaseAction = returnAction;
        CubeTouched += _releaseAction;
    }

    public void UnregisterReturnAction()
    {
        CubeTouched -= _releaseAction;
        _releaseAction = null;
    }

    private float SetRandomValye()
    {
        _runTime = Random.Range(_runTimeMin, _runTimeMax);
        return _runTime;
    }

    private IEnumerator CooldownDisable()
    {
        yield return new WaitForSeconds(SetRandomValye());
        CubeTouched?.Invoke();
    }
}
