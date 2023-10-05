using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class playerctrl : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject CanvasDeathLay;

    float speed;

    [SerializeField] float normalSpeed = 10f;
    [SerializeField] float jumpForce = 60f;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject inventory_canvas;
    [SerializeField] GameObject Damage_screen;


    bool isActivitiesStop = false;


    public Image bar;
    Vector2 moveHorizontal;
    private int startLifesAmount = 5; //Начальное количество жизней
    private int lifesAmount;
    public int LifesAmount              //При изменении количества жизней изменяется healthbar
    { get { return lifesAmount; }
        set 
        {
            if (value <= 0)
            {
                bar.fillAmount = 0;
                ClearPlayerActivities();
                CanvasDeathLay.SetActive(true);
                Invoke("ReloadPlayer", 2f);
                lifesAmount = 0;
            }
            else
            {
                bar.fillAmount = (float)value / startLifesAmount;
                lifesAmount = value;
            }
           

        }
    }

    [SerializeField] Transform groundCheck;
    [SerializeField] bool onGround;
    float checkRadius = 0.3f;
    [SerializeField] LayerMask Ground;



    void Start()
    {
        speed = 0f;
        lifesAmount = startLifesAmount; 
        rb = GetComponent<Rigidbody2D>();
        SetPlayerActivities();
        GlobalEventManager.OnPlayerActive += SetPlayerActivities;
        GlobalEventManager.OnPlayerStop += ClearPlayerActivities;
        GlobalEventManager.OnPlayerDied += ReloadPlayer;
        GlobalEventManager.OnGotDamage += GetDamage;
    }

    void Update()
    {
        if (!isActivitiesStop)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            CheckingGround();
        }
    }


    public void LeftButtonClick() 
    { 
        Debug.Log("loh");  
        if (speed >= 0f) 
        {
            speed = -normalSpeed; 
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0); 
        } 
    }
    public void RightButtonClick() { if (speed <= 0f) { speed = normalSpeed; gameObject.transform.rotation = Quaternion.Euler(0, 0, 0); } }
    public void OnButtonUp() { speed = 0f; }
 
    public void Walk()          //Хождение
    {
        moveHorizontal.x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveHorizontal.x * normalSpeed, rb.velocity.y);
        if (moveHorizontal.x < 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveHorizontal.x > 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }

    public void Jump()        
    {
        if (onGround)
        {          
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void Down() 
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        Invoke("IgnoreCollisionOff", 0.5f);
    }

    void CheckingGround()    //Проверка земли под ногами персонажа
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, Ground);
    }

    void IgnoreCollisionOff()           //Выклюлчение игнорирования платформы через время
    {
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))       //Столкновение со смертельной зоной
        {           
            StartCoroutine(waiter());
        }
        IEnumerator waiter()
        {
            yield return new WaitForSeconds(0.5f);
            LifesAmount = 0;                              
        }
        if (collision.gameObject.CompareTag("Inventory_Object"))       //Подбирание инвентаря
        {
            Debug.Log("Catch!");
            if (!collision.gameObject.GetComponent<Item_behavior>().isCollected)
            {
                Items item = ScriptableObject.CreateInstance<Items>();
                item.name = collision.gameObject.name;
                item.amount = 1;
                item.icon = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
                collision.gameObject.GetComponent<Item_behavior>().isCollected = true;
                Destroy(collision.gameObject);
                GlobalEventManager.SendItemTake(item);
            }
            
            
        }
    }

 

    void ReloadPlayer()   //перезапуск персонажа
    {      
        gameObject.transform.position = new Vector3(-6, 2, 0);
        LifesAmount = startLifesAmount;
        CanvasDeathLay.SetActive(false);
        SetPlayerActivities();
    }

    void ClearPlayerActivities()  //остановка персонажа
    {
        isActivitiesStop = true;
    }

    void SetPlayerActivities() 
    {
        isActivitiesStop = false;
    }



    void GetDamage(int damage)  //получение урона
    {
        LifesAmount = LifesAmount - damage;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * 40f, ForceMode2D.Impulse);
        StartCoroutine(waiter());
        IEnumerator waiter()
        {
            Damage_screen.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            Damage_screen.SetActive(false);
        }
    }

    public void UseGun()        //Стрельба
    {
        gun.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        GlobalEventManager.SendFire();
    }

}
