using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Kali : MonoBehaviour
{    
    private enum PlayerState
    {
        Attack,
        Die,
        Moving,
        Idle,
        Q,
        W,
        E,
        R,
    }
    private bool isAction = false;
    private bool canNormalAttack = true;
    private float canNormalAttackTime = 2f;
    private int AttackNum = 0;
    private Vector3 dir;

    PlayerState _state = PlayerState.Idle;
    private bool useRootMotion = false;
    private Animator animator;
    private Camera mainCamera;
    private NavMeshAgent agent;
    private Rigidbody rigid;

    public KailAni kailAni;

    //skill cooltime variable
    float t_time = 0.0f;
    float q_time = 0.0f;
    float w_time = 0.0f;
    float e_time = 0.0f;
    float r_time = 0.0f;
    bool press = false;
    WaitForSeconds seconds = new WaitForSeconds(0.1f);

    //sound variable
    public AudioSource[] audioSource;
    private bool MovingAudioSoungIsActive = false;

    private GameObject R_Skill;
    public GameObject R_Skill_Prefab;
    private Stat _stat = new Stat();

    void Activation(string skill)
    {
        switch(skill)
        {
            case "Q":
                q_time = 11.0f;
                break;

            case "W":
                w_time = 15.0f;
                break;

            case "E":
                e_time = 16.0f;
                break;

            case "R":
                r_time = 90.0f;
                break;
        }
        t_time = Mathf.Max(q_time, w_time, e_time, r_time, t_time);
        //이미 실행 중이라면
        if(press == true)
        {
        }
        //코루틴 처음 시작하면
        else
        {
            press = true;
            StartCoroutine(Skill_CoolTime());
        }
    }
    IEnumerator Skill_CoolTime()
    {
        while(t_time > 0)
        {
            if(q_time > 0.1f)
            {
                q_time -= 0.1f;
            }
            if(w_time> 0.1f)
            {
                w_time -= 0.1f;
            }
            if (e_time > 0.1f)
            {
                e_time -= 0.1f;
            }
            if (r_time > 0.1f)
            {
                r_time -= 0.1f;
            }
            if(t_time < 0.1f)
            {
                t_time = 0.1f;
                press = false;
            }
            t_time -= 0.1f;
            yield return seconds;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        rigid = GetComponent<Rigidbody>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    void Start()
    {
        _stat = ObjectPooling.instance.Get_Stat("kyle");

    }
    void OnAnimatorMove()
    {
        if (useRootMotion)
        {
            transform.rotation = animator.rootRotation;
            transform.position += animator.deltaPosition;
            //dir = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        }
    }
    void ChooseAction()
    {
        if (isAction)
            return;

        // Normal Attack
        Basic_Attack();

        //Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (q_time > 0.1f)
                return;
            //useRootMotion = true; 
            ChangRotate();
            Activation("Q");
            Determination();
            _state = PlayerState.Q;
        }
        //W

        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (w_time > 0.1f)
                return;
            useRootMotion = true; ChangRotate();
            Atonement();
            Activation("W");
            _state = PlayerState.W;
        }
        //E

        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (e_time > 0.1f)
                return;
            useRootMotion = true; ChangRotate();
            Activation("E");
            Evation();
            _state = PlayerState.E;
        }
        //R
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (r_time > 0.1f)
                return;
            useRootMotion = true; ChangRotate();
            Activation("R");
            HorizonofMemory();
            _state = PlayerState.R;
        }
    }
    void ChangRotate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            dir = hit.point;
            dir.y = transform.position.y;
            transform.LookAt(dir);
        }
    }
    #region Q_Skill
    void Determination()
    {
        agent.ResetPath();
        isAction = true;
        animator.SetBool("Moving", false);
        animator.SetBool("Skill", true);
        animator.SetInteger("SkillNumber", 1);
        audioSource[0].Play();
    }
    public void Q_Stop()
    {
        isAction = false;
        useRootMotion = false;
        animator.SetBool("Skill", false);
        _state = PlayerState.Idle;
    }
    #endregion
    #region W_Skill
    void Atonement()
    {
        agent.ResetPath();
        isAction = true;
        animator.SetBool("Moving", false);
        animator.SetBool("Skill", true);
        animator.SetInteger("SkillNumber", 2);
        audioSource[1].Play();
    }
    public void W_Stop()
    {
        isAction = false;
        useRootMotion = false;
        animator.SetBool("Skill", false);
        _state = PlayerState.Idle;
    }
    #endregion
    #region E_Skill
    void Evation()
    {
        agent.ResetPath();
        useRootMotion = true;
        isAction = true;
        animator.SetBool("Moving", false);
        animator.SetBool("Skill", true);
        animator.SetInteger("SkillNumber", 3);
        audioSource[2].Play();
    }
    public void E_Stop()
    {
        useRootMotion = false;
        isAction = false;
        animator.SetBool("Skill", false);
        _state = PlayerState.Idle;
    }
    #endregion
    #region R_Skill
    void HorizonofMemory()
    {
        agent.ResetPath();
        isAction = true;
        animator.SetBool("Moving", false);
        animator.SetBool("Skill", true);
        animator.SetInteger("SkillNumber", 4);
        audioSource[3].Play();
    }
    public void R_Sound()
    {
        audioSource[4].Play();
    }
    public void R_Instantiate()
    {
        R_Skill = Instantiate(R_Skill_Prefab);
        R_Skill.GetComponent<Kail_R>().Trigger(transform.position);
    }
    public void R_Stop()
    {
        isAction = false;
        useRootMotion = false;
        animator.SetBool("Skill", false);
        _state = PlayerState.Idle;
    }
    #endregion
    void Basic_Attack()
    {
        if (!canNormalAttack)
            return;
        if(Input.GetMouseButtonDown(0) && canNormalAttack)
        {
            if(AttackNum == 0)
            {
                animator.SetBool("Attack", true);
                animator.SetBool("NormalAttack", true);
                AttackNum = 1;
            }
            else
            {
                animator.SetBool("Attack", true);
                animator.SetBool("NormalAttack", false);
                AttackNum = 0;
            }
            audioSource[5].Play();
            StartCoroutine(CoolTime(canNormalAttackTime, canNormalAttack));
        }
    }
    IEnumerator CoolTime(float time, bool b)
    {
        b = !b;
        yield return new WaitUntil(() => (animator.GetCurrentAnimatorStateInfo(0).IsName("Normal Attack 1") || animator.GetCurrentAnimatorStateInfo(0).IsName("Normal Attack 2")) && !animator.IsInTransition(0));
        animator.SetBool("Attack", false);
        b = !b; 
    }
    void Update()
    {
        print(t_time);
        print(q_time);
        print(w_time);

        ChooseAction();
        switch(_state)
        {
            case PlayerState.Q:
            case PlayerState.W:
            case PlayerState.E:
            case PlayerState.R:
                transform.LookAt(dir);
                return;
            case PlayerState.Moving:
                UpdateMoving();
                break;

            case PlayerState.Die:
                UpdateDie();
                break;

            case PlayerState.Attack:
                break;

            case PlayerState.Idle:
                UpdateIdle();
                break;

        }
        //CharacterMovement();
        if(animator.GetBool("Moving") && !MovingAudioSoungIsActive)
        {
            MovingAudioSoungIsActive = true;
            StartCoroutine(MoveSound());
        }
        if(animator.GetBool("Moving") == false)
        {
            MovingAudioSoungIsActive = false;

            StopCoroutine(MoveSound());
        }
    }
    IEnumerator MoveSound()
    {
        audioSource[6].Play();
        yield return new WaitForSeconds(2f);
        MovingAudioSoungIsActive = false;

    }
    private void UpdateDie()
    {

    }
    private void UpdateIdle()
    {
        if (Input.GetMouseButtonDown(1))
        {
            agent.velocity = Vector3.zero;
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building");
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000,  mask))
            {
                dir = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                agent.SetDestination(dir);
                animator.SetBool("Moving", true);
                _state = PlayerState.Moving;
            }
        }
    }
    private void UpdateMoving()
    {
        dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z);
        if (dir.magnitude < 0.01f)
            _state = PlayerState.Idle;

        transform.LookAt(dir);

        if (Input.GetMouseButtonDown(1))
        {
            agent.velocity = Vector3.zero;
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Walkable") | LayerMask.GetMask("Building");
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000,  mask))
            {
                dir = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                agent.SetDestination(dir);
            }
        }
    }
}
