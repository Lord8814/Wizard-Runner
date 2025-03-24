using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour

{
    private Animator animator;
    private Vector3 lastPosition;
    private float speed;

    void Start()
    {
        // Récupérer l'Animator attaché au GameObject
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculer la vitesse en fonction du déplacement
        speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        // Mettre à jour le paramètre "Speed" dans l'Animator
        animator.SetFloat("Speed", speed);
        Debug.Log("Speed: " + speed);


    }
}

