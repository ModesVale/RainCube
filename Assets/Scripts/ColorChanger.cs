using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    public void SetRandomColor()
    {
        _meshRenderer.material.color = Random.ColorHSV();
    }

    public void SetDefaultColor()
    {
        _meshRenderer.material.color = Color.white;
    }
}