using UnityEngine;

public class UFOBody : MonoBehaviour
{
    public Control Control;
    public bool isCol = false;
    public int hp = 50;
    private Vector3 collisionPos;

    private void Start()
    {
        Invoke("GetControl", 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && !collision.gameObject.CompareTag("UFOBullet"))
        {
            collisionPos = collision.gameObject.transform.position;
            if (Control != null)
            {
                isCol = true;
                Control.UFOBodyCallback = CollisionPos;
            }
            ISColFalseInvoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("BeamHit"))
        {
            hp = hp -1;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Whale"))
        {
            hp = hp -50;
        }
        
    }
    private void OnEnable()
    {
        hp = 50;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BeamHit"))
        {
            hp = 0;
        }
        if (other.gameObject.CompareTag("Whale"))
        {
            hp = 0;
        }
    }

    private void GetControl()
    {
        Control = GetComponentInParent<Control>();
    }

    private Vector3 CollisionPos()
    {
        return collisionPos;
    }

    private void ISColFalseInvoke()
    {
        Invoke("ISColFalse", 1f);
    }

    private void ISColFalse()
    {
        isCol = false;
    }
}
