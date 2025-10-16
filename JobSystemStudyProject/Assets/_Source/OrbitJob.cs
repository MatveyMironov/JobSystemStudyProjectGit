using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

public struct OrbitJob : IJobParallelForTransform
{
    public NativeArray<float> RadiusArray;
    public NativeArray<float> AngularSpeedArray;
    public NativeArray<float> AngleArray;
    
    public float DeltaTime;
    
    public void Execute(int index, TransformAccess transform)
    {
        var radius = RadiusArray[index];
        var angularSpeed = AngularSpeedArray[index];
        
        var deltaAngle = angularSpeed * DeltaTime;
        var angle = AngleArray[index] + deltaAngle;
        
        while (angle > 360.0f)
            angle -= 360.0f;
        while (angle < 0)
            angle += 360.0f;
        
        AngleArray[index] = angle;
        
        var positionX = Mathf.Cos(angle) * radius;
        var positionZ = Mathf.Sin(angle) * radius;
        transform.position = new Vector3(positionX, transform.position.y, positionZ);
    }
}
