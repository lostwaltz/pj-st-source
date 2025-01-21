using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRemoldingElement : MonoBehaviour
{
    [SerializeField] private GameObject remold;
    
    public void SetRemolding(bool remolding)
    {
        remold.SetActive(remolding);
    }
}
