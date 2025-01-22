using UnityEngine;
using DG.Tweening;

public class BubbleControler : MonoBehaviour
{
    //TODO set max and min value
    [SerializeField] private float _targetSize;
    [SerializeField] private float _sizeChangeSpeed = 5;
    [SerializeField] private float _sizeDecayPerSecond = .05f;

    [Header("Min/Max : ")]
    [SerializeField] private float _minSize = 2;
    [SerializeField] private float _maxSize = 5;
    private SphereCollider _sphereCollider;

    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnValidate()
    {
        if (!_sphereCollider) Start();
        _sphereCollider.radius = _targetSize;
    }

    private void FixedUpdate()
    {
        _sphereCollider.radius = Mathf.Lerp(_sphereCollider.radius, _targetSize, Time.fixedDeltaTime * _sizeChangeSpeed);
        _targetSize -= _sizeDecayPerSecond * Time.fixedDeltaTime;
        _targetSize = Mathf.Clamp(_targetSize, _minSize, _maxSize);
    }

    public void UpdateSize(float sizeToAdd)
    {
        _targetSize += sizeToAdd;
        _targetSize = Mathf.Clamp(_targetSize, _minSize, _maxSize);
    }
}