using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    private bool isShoot = false;
    private VisualEffect vfx;
    private float bullet_DMG;
    private float speed;
    private Vector3 direction;
    private GameObject owner;
    public float Bullet_DMG { get { return bullet_DMG; } set {  bullet_DMG = value; } } 
    public GameObject Owner { get { return owner; } set { owner = value; } }
    public VisualEffect VFX { get { return vfx; } set { vfx = value; } }

    private void Awake()
    {
        vfx = GetComponent<VisualEffect>();
        vfx.SetFloat("Life Time", 10f);
    }
    private void Update()
    {
        if (isShoot)
        {
            this.transform.Translate(direction * speed * Time.deltaTime);
        }
    }
    private void DestroyFunction()
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        DestroyFunction();
    }
    public void Shoot(GameObject _owner, Vector3 _dir)
    {
        owner = _owner;
        direction = _dir;
        isShoot = true;

        Invoke("DestroyFunction", 5f);
    }

}
