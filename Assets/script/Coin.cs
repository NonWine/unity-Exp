
using UnityEngine;
public class Coin : MonoBehaviour
{
    //ALSO NEED PHYSICS MATERIAL
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private TrailRenderer trailRenderer;
    private Vector3 endPos;
    private Vector3 gravityPos;
    private bool collect;
    private bool trig;
    private bool canRotate;
    private float time;
    private bool gravityDown;
    private Vector3 off;
    void Start()
    {
        trailRenderer.enabled = false;
        off = new Vector3(Random.Range(-4f, 4f),-1.5f, Random.Range(-4f, 4f));
        endPos = transform.position + off;
    }

    private void Update()
    {

        if (!trig)
        {
            time += Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, endPos, time);
            if (transform.position == endPos)
            {
                trig = true;
                canRotate = true;
                sphereCollider.enabled = true;
                gravityPos = transform.position;
                time = 0f;
            }
        }
        else if (canRotate)
        {
            #region gravity
            //if (!gravityDown)
            //{
            //    time += Time.deltaTime;
            //    transform.position = Vector3.MoveTowards(transform.position, gravityPos + Vector3.up, time * 0.005f);
            //    if (transform.position == gravityPos + Vector3.up)
            //    {
            //        gravityDown = true;
            //        time = 0f;
            //    }

            //}
            //if (gravityDown)
            //{
            //    time += Time.deltaTime;
            //    transform.position = Vector3.MoveTowards(transform.position, gravityPos - Vector3.up, time * 0.005f);
            //    if(transform.position == gravityPos - Vector3.up)
            //    {
            //        gravityDown = false;
            //        time = 0f;
            //    }
            //}
            #endregion gravity
            transform.Rotate(Vector3.right * 3f);
        }

        if (collect)
        {
            trailRenderer.enabled = true;
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, Player.Instance.transform.position, time);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, time);
            if (transform.localScale == Vector3.zero)
            {
              
                Destroy(gameObject);
            }
               
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && trig)
            collect = true;
    }

}
