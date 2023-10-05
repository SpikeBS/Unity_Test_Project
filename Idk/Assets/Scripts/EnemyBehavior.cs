using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour
{
    delegate void ActionsWithhGoal();
    ActionsWithhGoal awg;
    GameObject player;
    public Image bar;

    [SerializeField] int damage = 1;

    [SerializeField] private int startEnemyLifesAmount = 10;
    private int enemyLifesAmount;
    public int EnemyLifesAmount
    {
        get { return enemyLifesAmount; }
        set
        {
            if (value <= 0)
            {
                bar.fillAmount = 0;
                awg += DeathAnim;
                
                Invoke("Death", 0.3f);
                enemyLifesAmount = 0;
            }
            else
            {
                bar.fillAmount = (float)value / startEnemyLifesAmount;
                enemyLifesAmount = value;
            }

        }
    }

    [SerializeField] float speed = 3f;
    float StartTime = 1;
    float WaitTime;

    [SerializeField][Range(0, 5)] int DeltaX = 3;
    float RandomXPosition;
    [SerializeField] float[] RandomPoints;
    float lastPositionX;

    float cooldown;
    float cooldownStart = 2f;
    [SerializeField] bool isCoolDownEnd = true;
    [SerializeField] GameObject sign_warning;

    Rigidbody2D rb;
    [SerializeField] Transform groundCheck;

    Vector3 CurrentGoal;
    float checkRadius = 0.5f;
    [SerializeField] LayerMask Ground;


    [System.Obsolete]
    void Start()
    {
        enemyLifesAmount = startEnemyLifesAmount;
        rb = GetComponent<Rigidbody2D>();   
        awg = GoToRandomPoint;
        player = GameObject.FindGameObjectWithTag("Player");
        RandomPoints = new float[3] { transform.position.x - DeltaX, transform.position.x, transform.position.x + DeltaX };
        lastPositionX = transform.position.x;
        RandomXPosition = RandomPoints[Random.RandomRange(0, RandomPoints.Length)];
        cooldown = cooldownStart;
        CurrentGoal = new Vector3(RandomXPosition, transform.position.y, transform.position.z);
    }

    [System.Obsolete]
    void Update()
    {
        
        awg?.Invoke();



        lastPositionX = transform.position.x;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        rb.velocity = Vector2.zero;
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isCoolDownEnd)
            {
                GlobalEventManager.SendGotDamage(damage);
                CoolDownWaiter();
            }            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            sign_warning.SetActive(true);
            rb.velocity = new Vector2(0, 0);
            //StartCoroutine(waiter());          
            Invoke("SetAwg", 0.5f);
        }
        IEnumerator waiter()
        {
            rb.velocity = new Vector2(0, 0);
            yield return new WaitForSeconds(1f);
            awg = FollowPlayer;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(waiter());
        }
        IEnumerator waiter()
        {
            rb.velocity = new Vector2(0, 0);
            sign_warning.SetActive(false);
            yield return new WaitForSeconds(2f);
            CurrentGoal = new Vector3(RandomXPosition, transform.position.y, transform.position.z);
            awg = GoToRandomPoint;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            GetDamage(1);
        }
    }

    void CoolDownWaiter() 
    {
        isCoolDownEnd = false;
        StartCoroutine(waiter());
        IEnumerator waiter()
        {
            while (cooldown > 0)
            {
                cooldown -= 1;
                Debug.Log(cooldown);
                yield return new WaitForSeconds(1);                
            }
            if (cooldown <= 0)
            {
                cooldown = cooldownStart;
                Debug.Log("END");
                isCoolDownEnd = true;
            }
        }       
    }


    void GoToRandomPoint()       //случайное передвижение
    {
        if (Vector2.Distance(transform.position, CurrentGoal) < 0.5f && CheckingGround())
        {
            if (WaitTime <= 0)
            {               
                RandomXPosition = RandomPoints[Random.Range(0, RandomPoints.Length)];
                CurrentGoal = new Vector3(RandomXPosition, transform.position.y, transform.position.z);
                WaitTime = StartTime;
            }
            else
            {
                WaitTime -= Time.deltaTime;
            }
        }
        else
            transform.position = Vector2.MoveTowards(transform.position, CurrentGoal, speed * Time.deltaTime);
    }

    void FollowPlayer()                //преследование игрока
    {
        CurrentGoal = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        if (Vector2.Distance(transform.position, CurrentGoal) > 0.5f && CheckingGround())
            transform.position = Vector2.MoveTowards(transform.position, CurrentGoal, speed * Time.deltaTime);
    }


    void SetAwg()
    {
        awg = FollowPlayer;
    }

    bool CheckingGround()    //проверка земли под ногами
    {
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, Ground);
    }


    void GetDamage(int damage)  //получение урона
    {
        EnemyLifesAmount = EnemyLifesAmount - damage;
        rb.velocity = Vector2.zero;
        //rb.AddForce(Vector2.up * 60f, ForceMode2D.Impulse);
    }

    void DeathAnim() 
    {
        gameObject.transform.localScale = new Vector3(transform.localScale.x + 0.05f, transform.localScale.y, transform.localScale.z);
    }

    void Death() 
    {
        Destroy(gameObject);
    }

}
