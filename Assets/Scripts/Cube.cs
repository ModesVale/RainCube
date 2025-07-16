using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class Cube : MonoBehaviour
{
    [SerializeField] private float _runTimeMin = 2f;
    [SerializeField] private float _runTimeMax = 5f;

    private float _runTime;
    private bool _isTouchedPlatform = false;
    private UnityAction _releaseAction;
    private ColorChanger _colorChanger;
    public event UnityAction CubeTouched;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
    }

    public void SetDefaultConfig()
    {
        _isTouchedPlatform = false;
        _colorChanger.SetDefaultColor();
        transform.rotation = Quaternion.identity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Platform _) && _isTouchedPlatform == false)
        {
            _isTouchedPlatform = true;
            _colorChanger.SetRandomColor();
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

    private float ReturnRandomValye()
    {
        _runTime = Random.Range(_runTimeMin, _runTimeMax);
        return _runTime;
    }

    private IEnumerator CooldownDisable()
    {
        yield return new WaitForSeconds(ReturnRandomValye());
        CubeTouched?.Invoke();
    }
}
