using System.Linq;
using UnityEngine;

public class UnitRequirement : MonoBehaviour
{
    // 숏컷 제공
    [Header("Shortcut")] 
    public Transform firePoint;
    public Transform cameraPoint;
    public Transform rightHand;
    public Transform hpPoint;

    public void FindShorcut()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        firePoint = transforms.FirstOrDefault(t => t.name == "lgp_muzzle");
        cameraPoint = transforms.FirstOrDefault(t => t.name == "CameraPoint");
        rightHand = transforms.FirstOrDefault(t => t.name == "ThumbFinger2_R");
        hpPoint = transforms.FirstOrDefault(t => t.name == "HPPoint");
        
        if (firePoint == null)
        {
            GameObject gameObject = new GameObject("FirePosition");
            gameObject.transform.SetParent(transform);
            gameObject.transform.position = transform.position + new Vector3(0, 0.5f, 0) + transform.forward;
            
            firePoint = gameObject.transform;
        }
        if (rightHand == null)
        {
            GameObject gameObject = new GameObject("RightHand");
            gameObject.transform.SetParent(transform);
            gameObject.transform.position = transform.position + new Vector3(0, 0.5f, 0) + transform.forward;
            
            rightHand = gameObject.transform;
        }
        if (hpPoint == null)
        {
            GameObject gameObject = new GameObject("HPPoint");
            gameObject.transform.SetParent(transform);
            gameObject.transform.position = transform.position + new Vector3(0, 2f, 0) + transform.forward;
            
            hpPoint = gameObject.transform;
        }
    }
}
