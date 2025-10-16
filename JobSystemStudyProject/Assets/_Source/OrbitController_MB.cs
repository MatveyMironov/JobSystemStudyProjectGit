using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

public class OrbitController_MB : MonoBehaviour
{
    [SerializeField] private Transform _objectPrefab;
    [SerializeField] private int objectsCount;

    [Space]
    [SerializeField] private float maxRadius;
    [SerializeField] private float minRadius;
    [SerializeField] private float thickness;
    [Tooltip("Speed of object at radius 1")]
    [SerializeField] private float speedFactor;
    [Tooltip("How much radius affects speed")]
    [SerializeField] private float radiusFactor;
    
    private NativeArray<float> _radiusArray;
    private NativeArray<float> _angularSpeedArray;
    private NativeArray<float> _angleArray;
    private TransformAccessArray _transformAccessArray;
    
    private void Start()
    {
        _radiusArray = new NativeArray<float>(objectsCount, Allocator.Persistent);
        _angularSpeedArray = new NativeArray<float>(objectsCount, Allocator.Persistent);
        _angleArray = new NativeArray<float>(objectsCount, Allocator.Persistent);
        
        var transforms =  new Transform[objectsCount];
        for (int i = 0; i < objectsCount; i++)
        {
            float radius = UnityEngine.Random.Range(minRadius, maxRadius);
            float yPos = UnityEngine.Random.Range(-thickness / 2.0f, thickness / 2.0f);
            float angle = UnityEngine.Random.Range(0.0f, 360.0f);
            
            Vector3 position = new Vector3(radius, yPos, 0.0f);
            transforms[i] = Instantiate(_objectPrefab, position, Quaternion.identity);
            
            _radiusArray[i] = radius;
            _angularSpeedArray[i] = speedFactor * Mathf.Sqrt(1.0f / Mathf.Pow(radius, radiusFactor));
            _angleArray[i] = angle;
        }
        _transformAccessArray = new TransformAccessArray(transforms);
    }

    private void Update()
    {
        var orbitJob = new OrbitJob()
        {
            RadiusArray = _radiusArray,
            AngularSpeedArray = _angularSpeedArray,
            AngleArray = _angleArray,
            DeltaTime = Time.deltaTime,
        };
        var orbitHandle = orbitJob.Schedule(_transformAccessArray);
        orbitHandle.Complete();
    }

    private void OnDestroy()
    {
        _radiusArray.Dispose();
        _angularSpeedArray.Dispose();
        _angleArray.Dispose();
        _transformAccessArray.Dispose();
    }
}
