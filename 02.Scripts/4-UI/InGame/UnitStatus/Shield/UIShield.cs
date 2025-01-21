using System;
using UnityEngine;
using UnityEngine.UI;

public class UIShield : UIBase
{
    [SerializeField] private Image shieldImage;
    
    private UnitShieldSystem shieldSystem;
    
    public void SetInfo(Unit owner)
    {
        shieldSystem = owner.ShieldSystem;

        shieldSystem.OnValueChange -= OnValueChanged;
        shieldSystem.OnValueChange += OnValueChanged;
    }

    private void OnValueChanged(float per)
    {
        shieldImage.fillAmount = per;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            shieldSystem.ChargeShield(10, 10);
    }
}
