using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : Fighter
{
    private Vector3 originalSize;
    
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    

    public bool move = false;
    public float ySpeed = 0.75f;
    public float xSpeed = 1.0f;

    protected virtual void Start()
    {
        originalSize = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();
   }

    protected virtual void UpdateMotor(Vector3 input)
    {
        //Reset MoveDelta
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        //Troca de sprite de direção da direita pra esquerda ou vice versa de cima para baixo ou vice versa
        if (moveDelta.x > 0)
        {
            transform.localScale = originalSize;
        }
        else if (moveDelta.x < 0)
        {
            transform.localScale = new Vector3(originalSize.x * -1, originalSize.y, originalSize.z);
        }
       

        // Adiciona um vetor push, se existir algum
        moveDelta += pushDirection;

        // Reduz a força do push a cada frame, baseado no recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        // Verifica se podemos nos mover na direção escolhida, colocando uma caixa lá para nos se a caixa retornar nulo estamos livres para nos mover 
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Ator", "Bloqueio"));
        if (hit.collider == null)
        {
            //Fazendo a coisa se mover
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        // Verifica se podemos nos mover na direção escolhida, colocando uma caixa lá para nos se a caixa retornar nulo estamos livres para nos mover 
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Ator", "Bloqueio"));
        if (hit.collider == null)
        {
            //Fazendo a coisa se mover
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }
}
