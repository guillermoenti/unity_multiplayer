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
    private float jump;
    [SerializeField]
    private float fireate;


    [SerializeField]
    private float jumpForce = 800f;

    private Rigidbody2D rb;
    private float desiredMovementAxis = 0f;

    private PhotonView pv;
    private Vector3 enemyPosition = Vector3.zero;

    public TextMeshProUGUI usertext;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();

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
        usertext.text = speed.ToString();

        

    }

    private void Update()
    {
        if (pv.IsMine) { CheckInputs(); }
        else { Replicate(); }
     
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(desiredMovementAxis * Time.fixedDeltaTime * speed, rb.velocity.y);
    }

    private void CheckInputs()
    {
        desiredMovementAxis = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(0f, jumpForce));
        }

        //Compruebo si quiero disparar
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Shoot();
        }
    }

    //Instancio por la red mi bala
    private void Shoot()
    {
        //Aqui no tenemos en cuenta donde mira el personaje ni spawns de bala, siempre se ira hacia la derecha
        PhotonNetwork.Instantiate("Bullet", transform.position + new Vector3(1f, 0f, 0f), Quaternion.identity);
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
        Race race = DataManager.instance.races[DataManager.instance.playerRaceId];

        health = race.health;
        damage = race.damage;
        speed = race.speed * 200;
        jump = race.jump;
        fireate = race.firerate;
    }
}
