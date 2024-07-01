using UnityEngine;

public interface IWeapon
{
    public int Range { get { return Range; } set { Range = value; } }
    public float Damage { get { return Damage; } set { Damage = value; } }
    public float Speed { get { return Speed; } set { Speed = value; } }
    public bool ContinuousFire { get { return ContinuousFire; } set { ContinuousFire = value; } }
    public GameObject Bullet { get { return Bullet; } set { Bullet = value; } }
    public void Fire(Transform[] shootPositions, Transform target);
}
