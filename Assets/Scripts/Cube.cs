using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private int _minLifeTime = 1;
    private int _maxLifeTime = 2;

    private int LifeTime => UnityEngine.Random.Range(_minLifeTime, _maxLifeTime + 1);

    public event Action<Cube> CubeRemove;

    public void OnRemove()
    {
        CubeRemove?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Floor floor))
        {
            DestroyInTime(LifeTime);
        }
    }

    private IEnumerator DestroyInTime(int delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
