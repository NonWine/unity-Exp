using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private Animator _myAnim;
    [SerializeField] private Transform _bodyToRotate;
    [SerializeField] private Detector _detector;
    private AttackControll attackControll;
    private bool canRotate = true;
    private NavMeshAgent agent;
    private Vector3 direction;
    private bool canMove;

    private void Awake()
    {
        canMove = true;
        agent = GetComponent<NavMeshAgent>();
        attackControll = GetComponent<AttackControll>();
    }

    private void Update()
    {
        if (canMove)
        {
            Move();
            Rotate();
            UpdateAnimator();
        }

    }

    private void Move()
    {
         direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized;
         agent.Move(direction * (agent.speed * Time.deltaTime));
    }

    private void Rotate()
    {
        _detector.TryFindTheNearlestEnemy(_bodyToRotate.position);
        if (_detector.isDetected())
        {

            _detector.setPointInfinity();
            Vector3 dir = _detector.GetTargetInfo().position - _bodyToRotate.transform.position;
            Quaternion rot = Quaternion.Slerp(_bodyToRotate.rotation, Quaternion.LookRotation(dir), 12 * Time.deltaTime);
            _bodyToRotate.rotation = rot;

        }
        else
        {
            if (direction != Vector3.zero && canRotate)
                _bodyToRotate.rotation = Quaternion.Slerp(_bodyToRotate.rotation, Quaternion.LookRotation(direction), agent.angularSpeed * Time.deltaTime);

        }

    }

    private void UpdateAnimator() 
    {
        if (_detector.isDetected())
        {
            _myAnim.SetLayerWeight(0, 1f);
            _myAnim.SetLayerWeight(1, 1f);
        }

        else
            _myAnim.SetLayerWeight(1, 0f);
        _myAnim.SetFloat("speed", Vector3.ClampMagnitude(direction, 1).magnitude);
    } 

    public Animator PlayerAnim() { return _myAnim; }

    public Transform GetBody() { return _bodyToRotate; }

    public void ChangeAbilityToRotate(bool flag) => canRotate = flag;

    public void Attack()
    {
        _myAnim.SetTrigger("Attack");
        attackControll.MelleAttack();
    }


    public void LockMove()
    {
        canMove = false;
        _myAnim.SetFloat("speed", 0f);
    }

    public void UnlockMove()
    {
        canMove = true;
        Time.timeScale = 1f;
    }
}
