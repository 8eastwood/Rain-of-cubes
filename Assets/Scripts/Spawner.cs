using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<Cube> _pool;
    private bool _isWorking = true;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(CreateCube, GetFromPool, ReleaseInPool, Destroy, true, _poolCapacity, _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnCubeWithRate(_repeatRate));
    }

    private void GetFromPool(Cube cube)
    {
        int minStartPointX = 15;
        int maxStartPointX = 25;
        int minStartPointZ = -10;
        int maxStartPointZ = 10;
        int startPointY = 15;

        cube.transform.position = new Vector3(Random.Range(minStartPointX, maxStartPointX), startPointY, Random.Range(minStartPointZ, maxStartPointZ));
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.gameObject.SetActive(true);
    }

    private void ReleaseInPool(Cube cube)
    {
        cube.gameObject.SetActive(false);
    }

    private Cube CreateCube()
    {
        Cube cube = Instantiate(_cubePrefab);

        return cube;
    }

    private void RemoveCube(Cube cube)
    {
        _pool.Release(cube);
        cube.Removed -= RemoveCube;
    }

    private IEnumerator SpawnCubeWithRate(float repeatRate)
    {
        var wait = new WaitForSeconds(repeatRate);

        while (_isWorking)
        {
            yield return wait;

            GetCube();
        }
    }

    private void GetCube()
    {
        Cube newCube = _pool.Get();
        newCube.Removed += RemoveCube;
    }
}
