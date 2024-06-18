using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private List<Material> _colors = new List<Material>();
    [SerializeField] private Material _startMaterial;

    private int LifeTime => UnityEngine.Random.Range(_minLifeTime, _maxLifeTime + 1);
    private int _minLifeTime = 2;
    private int _maxLifeTime = 6;
    private MeshRenderer _renderer;
    private bool _colorChanged = false;

    public event Action<Cube> Removed;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        ResetMaterial();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Floor floor) && _colorChanged == false)
        {
            Debug.Log("collision happened");
            SetRandomColor(_renderer);
            StartCoroutine(DestroyInTime(LifeTime));
        }
    }

    private IEnumerator DestroyInTime(int delay)
    {
        yield return new WaitForSeconds(delay);
        Removed?.Invoke(this);
        ResetMaterial();
    }

    private void ResetMaterial()
    {
        _colorChanged = false;
        _renderer.material = _startMaterial;
    }

    private void SetRandomColor(MeshRenderer meshRenderer)
    {
        int minColorNumber = 0;
        int randomColor = UnityEngine.Random.Range(minColorNumber, _colors.Count);
        meshRenderer.material = _colors[randomColor];
        Debug.Log("Color is changed");
        _colorChanged = true;
    }
}
