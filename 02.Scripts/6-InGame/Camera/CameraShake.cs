using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float ShakeAmount;
    float shakeTime;
    Vector3 initialPosition;
    Transform cameraPosition;
    
    void Start()
    {
        if (Camera.main != null) cameraPosition = Camera.main.transform;
        initialPosition = new Vector3(cameraPosition.position.x,cameraPosition.position.y, cameraPosition.position.z);
    }

    void Update() //구독됐을때, 코루틴으로 변경예정 
    {
        if (shakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * ShakeAmount + initialPosition;
            shakeTime -= Time.deltaTime;
        }
        else
        {
            shakeTime = 0.0f;
            transform.position = initialPosition;
        }
    }

    public void VibrateForTime(float time)
    {
        shakeTime = time;
    }
}
