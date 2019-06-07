using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    // Experiencia
    public int xpValue = 1;

    // Logica
    public float triggerLenght = 1;
    public float chaseLenght = 5;

    private Animator anima;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    // HitBox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        anima = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // O jogador esta no range?
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLenght)
        {
            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLenght)
            {
                chasing = true;
                anima.SetBool("FindPlayer", true);
            }

            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    UpdateMotor((playerTransform.position - transform.position).normalized);
                }
            }
            else
            {
                UpdateMotor(startingPosition - transform.position);
                anima.SetBool("BossAttack", true);
                
            }
        }
        else
        {
            UpdateMotor(startingPosition - transform.position);
            chasing = false;
            anima.SetBool("FindPlayer", false);
            anima.SetBool("BossAttack", false); 
            
        }

        // Checando se o contato com o jogador aconteceu
        collidingWithPlayer = false;
        //Trabalho da Colisão
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            if(hits[i].tag == "Fighter" && hits[i].name == "Player")
            {
                collidingWithPlayer = true;
            }
           
            // Este array não se limpa, então deve ser limpado manualmente
            hits[i] = null;
        }
    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + " xp", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
    }
}
