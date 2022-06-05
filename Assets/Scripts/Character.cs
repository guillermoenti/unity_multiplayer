using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character : MonoBehaviour, IPunObservable
{

    [Header("Stats")]
    [SerializeField]
    private int health;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float fireate;

    private float timer;

    private Rigidbody2D rb;
    private float desiredMovementAxis = 0f;

    private bool isGrounded;
    private bool isFlipped = false;

    private PhotonView pv;
    private Vector3 enemyPosition = Vector3.zero;

    private Animator anim;

    public TextMeshProUGUI usertext;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();

        anim = GetComponent<Animator>();

        usertext = GetComponentInChildren<TextMeshProUGUI>();

        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 20;
    }

    private void Start()
    {
        SetStats();

        //Get nickname
        string nickname = GetComponent<PhotonView>().Owner.NickName;
        //Set name
        if (pv.IsMine)
        usertext.text = nickname;
        else
            usertext.text = DataManager.instance.enemyNickname;

        timer = 0;

    }

    private void Update()
    {
        if (pv.IsMine) 
        { 
            

            CheckInputs();

            if (isFlipped && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * - 1, transform.localScale.y, transform.localScale.z);
                usertext.transform.localScale = new Vector3(usertext.transform.localScale.x * -1, usertext.transform.localScale.y, usertext.transform.localScale.z);
            }
            else if(!isFlipped && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                usertext.transform.localScale = new Vector3(usertext.transform.localScale.x * -1, usertext.transform.localScale.y, usertext.transform.localScale.z);
            }

            timer += Time.deltaTime;
        }
        else
        {
            Replicate();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(desiredMovementAxis * Time.fixedDeltaTime * speed, rb.velocity.y);
        CheckGrounded();
    }

    private void CheckInputs()
    {
        desiredMovementAxis = Input.GetAxisRaw("Horizontal");
        anim.SetBool("isWalking", desiredMovementAxis != 0);
        if(desiredMovementAxis > 0)
        {
            isFlipped = false;
        }
        else if(desiredMovementAxis < 0)
        {
            isFlipped = true;
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            anim.SetTrigger("jump");
        }



        //Compruebo si quiero disparar
        if (Input.GetKeyDown(KeyCode.LeftControl) && timer >= fireate)
        {
            Shoot();
            timer = 0;
        }
    }

    //Instancio por la red mi bala
    private void Shoot()
    {
        //Aqui no tenemos en cuenta donde mira el personaje ni spawns de bala, siempre se ira hacia la derecha
        if (isFlipped)
        {
            PhotonNetwork.Instantiate(DataManager.instance.playerRace.raceName + "_Bullet", transform.position + new Vector3(-1f, 0f, 0f), Quaternion.Euler(0, 0, 180));
        }
        else
        {
            PhotonNetwork.Instantiate(DataManager.instance.playerRace.raceName + "_Bullet", transform.position + new Vector3(1f, 0f, 0f), Quaternion.identity);
        }
        
    }

    private void Replicate()
    {
        transform.position = enemyPosition;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {

        if (stream.IsWriting) {

            stream.SendNext(transform.position);

        } else if(stream.IsReading){
            enemyPosition = (Vector3)stream.ReceiveNext();
        }
    }

    //Funcion que se llamara como conductor a la funcion RPC
    public void Damage()
    {
        //Reproduzco el daño (en este caso destruyo directamente) a todos los clientes. Photon ya diferencia automaticamente a que prefab debe ejecutarlo
        pv.RPC("NetworkDamage", RpcTarget.All);
    }

    //Reproduce en todos los clientes el destruir
    [PunRPC]
    public void NetworkDamage()
    {
        Destroy(this.gameObject);
    }


    private void SetStats()
    {
        Race race = DataManager.instance.playerRace;

        health = race.health;
        damage = race.damage;
        speed = race.speed * 200;
        jumpForce = race.jump * 100;
        fireate = race.firerate;
    }

    private void CheckGrounded()
    {

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, 3);
        //Debug.Log(isGrounded);

        anim.SetBool("isGrounded", isGrounded);
    }
}
