using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int maxHp)
    {
        slider.maxValue = maxHp;
        SetHealth(maxHp);
    }
    
    public void SetMaxHealth(int maxHp, int curHp)
    {
        slider.maxValue = maxHp;
        SetHealth(curHp);

        fill.color = gradient.Evaluate(1f);
    }
    
    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
