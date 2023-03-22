using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackControll : MonoBehaviour
{
    [SerializeField] private float radiusMelleAttack;
    [SerializeField] private MobStats stats;
    private GameObject closestObjject;
    private float maxDistant;
    
    private void Start()
    {
        maxDistant = Mathf.Infinity;
    }

    public void MelleAttack()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, radiusMelleAttack);
        Debug.Log(colls.Length);
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].CompareTag("Enemy"))
            {
                Vector3 dir = colls[i].transform.position - transform.position;
                if (maxDistant >= dir.magnitude)
                {
                    maxDistant = dir.magnitude;
                    closestObjject = colls[i].gameObject;
                }

            }
           
        }
        if(closestObjject != null)
        {
            closestObjject.GetComponent<Character>().GetDamage(stats.GetDamage());
            RotateToObject(closestObjject.transform);
            maxDistant = Mathf.Infinity;
            closestObjject = null;
        }
    }

    private void RotateToObject(Transform obj)
    {
        transform.LookAt(obj,transform.forward);
    }


}
