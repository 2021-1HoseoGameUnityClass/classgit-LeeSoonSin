using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour // 모노비헤비어는 기본동작을 상속받는 느낌임.
{
    [SerializeField] // private로 해도 에디터창에 보이게 설정.
    private float moveSpeed = 3f;
    //점프관련변수
    [SerializeField]
    private float jumpForce = 300f;

    private bool isJump = false;

    //총알 발사 관련 변수
    [SerializeField]
    private GameObject bulletPos = null;

    [SerializeField]
    private GameObject bulletObj;

    private bool move = false;
    private float moveHorizontal = 0;

    private void Update()
    {
        if(move == true)
        {
            PlayeMove();
        }
        if(Input.GetButtonDown("Jump"))
        {
            PlayerJump();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    private void PlayeMove()
    {
        //float h = Input.GetAxis("Horizontal");
    float h = moveHorizontal;
    float playerSpeed = h * moveSpeed * Time.deltaTime;

    Vector3 vector3 = new Vector3();
    vector3.x = playerSpeed;
        transform.Translate(vector3);

        if(h< 0)
        {
            GetComponent<Animator>().SetBool("Walk", true);
    transform.localScale = new UnityEngine.Vector3(-1,1,1);
        }
        else if(h==0)
        {
            GetComponent<Animator>().SetBool("Walk", false);
        }
        else
        {
            GetComponent<Animator>().SetBool("Walk", true);
transform.localScale = new UnityEngine.Vector3(1, 1, 1);
        }
    }
    private void PlayerJump()
    {
        if(isJump == false)
        {
            //애니메이션 처리부
            GetComponent<Animator>().SetBool("Walk", false);
            GetComponent<Animator>().SetBool("Jump", true);
            //점프만큼 Add Force
            Vector2 vector2 = new Vector2(0, jumpForce);
            GetComponent<Rigidbody2D>().AddForce(vector2);
            isJump = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //충돌체의 콜라이더가 플랫폼 태그라면
        if(collision.collider.tag == "Platform")
        {
            GetComponent<Animator>().SetBool("Jump", false);
            isJump = false;
        }
    }
    private void Fire()
    {
        AudioClip audioClip = Resources.Load<AudioClip>("RangedAttack") as AudioClip;
        GetComponent<AudioSource>().clip = audioClip;
        GetComponent<AudioSource>().Play();
        float direction = transform.localScale.x;
        Quaternion quaternion = new Quaternion(0, 0, 0, 0);
        Instantiate(bulletObj, bulletPos.transform.position, quaternion).GetComponent<Bullet>().InstantiateBullet(direction);
    }

    public void OnMove(bool _right)
    {
        if(_right)
        {
            moveHorizontal = 1;
        }
        else
        {
            moveHorizontal = -1;
        }
        move = true;
    }
    public void OffMove()
    {
        moveHorizontal = 0;
        move = false;
    }
}
