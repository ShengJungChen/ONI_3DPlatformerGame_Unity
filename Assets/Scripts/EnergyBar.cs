using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider slider;
    public RectTransform rt;

    public void SetMaxHealth(float energy)
    {
        slider.maxValue = energy;
        slider.value = energy;
        rt.sizeDelta = new Vector2(energy * 5, 60);
    }

    public void SetHealth(float energy)
    {
        slider.value = energy;
    }
}
