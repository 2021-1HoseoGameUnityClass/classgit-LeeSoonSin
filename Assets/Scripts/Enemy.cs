using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private GameObject rayPos = null;
    [SerializeField]
    private int HP = 3;

    private bool moveRight = true;
    private bool isDead = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckRay();
    }
    private void CheckRay()
    {
        if(isDead == false)
        {
            //레이어 마스크
            LayerMask layerMask = new LayerMask();
            layerMask = LayerMask.GetMask("Platform");
            
            //레이캐스트
            RaycastHit2D ray = Physics2D.Raycast(rayPos.transform.position, new Vector2(0, -1), 1.1f, layerMask.value);

            Debug.DrawRay(rayPos.transform.position, new Vector3(0,-1,0), Color.red);

            //히트가 되지 않으면
            if(ray == false)
            {
                Debug.Log("반대방향으로");
                if(moveRight == true)
                {
                    moveRight = false;
                }
                else
                {
                    moveRight = true;
                }
            }
            Move();
        }
    }

    private void Move()
    {
        float direction = 0;
        if(moveRight)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        //플립
        Vector3 vector3 = new Vector3(direction, 1, 1);
        transform.localScale = vector3;

        //이동
        float speed = moveSpeed * Time.deltaTime * direction;
        vector3 = new Vector3(speed, 0,0);
        transform.Translate(vector3);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool cheak = collision.CompareTag("Bullet");
        if (cheak)
        {
            HP -= 1;
            if(HP < 1)
            {
                GetComponent<Animator>().SetBool("Death", true);
                isDead = true;
                Destroy(this.gameObject, 1);
            }
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            DataManager.instance.playerHP -= 1;
            if(DataManager.instance.playerHP < 0)
            {
                DataManager.instance.playerHP = 0;
            }
            UIManager.instance.PlayerHP();
        }
    }
}
