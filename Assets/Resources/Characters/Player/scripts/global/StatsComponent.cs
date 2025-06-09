using UnityEngine;

[System.Serializable]
public class StatsComponent : MonoBehaviour
{
    public int maxHp;
    public int str;
    public int agi;
    public float spd;
    public int def;
    public int mgcDef;
    public int atkSpeed;
    public int value;

    public void Initialize(int maxHp, int str, int agi, float spd, int def, int mgcDef, int atkSpeed)
    {
        this.maxHp = maxHp;
        this.str = str;
        this.agi = agi;
        this.spd = spd;
        this.def = def;
        this.mgcDef = mgcDef;
        this.atkSpeed = atkSpeed;
    }
}
