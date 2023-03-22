using UnityEngine;

[CreateAssetMenu(fileName = "MobStats", menuName = "ScriptableObject/Mob", order = 1)]
public class MobStats : ScriptableObject
{
    [SerializeField] private int _speed;
    [SerializeField] private int _Damage;
    [SerializeField] private float _CoolDown;
    [SerializeField] private int health;
    private int level;
    public void SetDefaltPlayer()
    {
        _speed = PlayerPrefs.GetInt("PlayerSpeed",0);
        _Damage = PlayerPrefs.GetInt("PlayerDamage",21);
        _CoolDown = PlayerPrefs.GetFloat("PlayerCD", 0.6f);
        health = PlayerPrefs.GetInt("PlayerHealth", 200);
    }

    public void SetDefaltNecromant()
    {
        _speed = PlayerPrefs.GetInt("NecromantSpeed", 9);
        _Damage = PlayerPrefs.GetInt("NecromantDamage", 15);
        _CoolDown = PlayerPrefs.GetFloat("NecromantCD", 0.8f);
        health = PlayerPrefs.GetInt("NecromantHealth", 2000);
    }

    public void SetDefaltRunner()
    {
        _speed = PlayerPrefs.GetInt("RuunerSpeed", 15);
        _Damage = PlayerPrefs.GetInt("RuunerDamage", 3);
        _CoolDown = PlayerPrefs.GetFloat("RuunerCD", 0.2f);
        health = PlayerPrefs.GetInt("RuunerHealth", 70);
    }

    public void SetDefaultZombie()
    {
        _speed = PlayerPrefs.GetInt("ZombieSpeed", 1);
        _Damage = PlayerPrefs.GetInt("ZombieDamage", 0);
        _CoolDown = PlayerPrefs.GetFloat("ZombieCD", 0.5f);
        health = PlayerPrefs.GetInt("ZombieHP", 100);
    }

    public void SetDefaultKnight()
    {
        _speed = PlayerPrefs.GetInt("KnightSpeed", 12);
        _Damage = PlayerPrefs.GetInt("KnightDamage", 25);
        _CoolDown = PlayerPrefs.GetFloat("KnightCD", 0.7f);
        health = PlayerPrefs.GetInt("KnightHP", 20);
    }

    public void SetDefaultArcher()
    {
        _speed = PlayerPrefs.GetInt("ArcherSpeed", 12);
        _Damage = PlayerPrefs.GetInt("ArcherDamage", 10);
        _CoolDown = PlayerPrefs.GetFloat("ArcherCD", 1f);
        health = PlayerPrefs.GetInt("ArcherHP", 200);
        level = PlayerPrefs.GetInt("ArcherLevel", 1);
    }

    public void levelUp()
    {
        level++;
    }

    public int GetLevel() { return level; }

    public int GetSpeed() { return _speed; }
    public int GetDamage() { return _Damage; }
    public float GetCD() { return _CoolDown; }

    public void ReduceCD(float value) { _CoolDown -= value; }

    public void AddDamage(int value)
    {
        _Damage += value;

    }

    public void AddSpeed(int value) => _speed += value;

    public void ResetStats()
    {
        _Damage = 35;
        _CoolDown = 0.4f;
    }

    

    public void AddHealth(int value) => health += value;

    public void ReduceHealth(int value) => health -= value;

    public int GetHealth() { return health; }

    public void SetHealth(int value) { health = value; }
}
