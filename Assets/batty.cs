using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batty : MonoBehaviour
{
    
        public Transform pointA;
        public Transform pointB;
        public float speed = 2f;
        public SpriteRenderer spriteRenderer;
        public Animator animator;
        public GameObject projectilePrefab;
        public Transform projectileSpawnPoint;

        private Transform targetPoint;
        private float attackCooldown = 5f;
        private float attackTimer;

        void Start()
        {
            targetPoint = pointB;
            attackTimer = attackCooldown;
        }

        void Update()
        {
            Patrol();
            HandleAttack();
        }

        void Patrol()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                targetPoint = targetPoint == pointA ? pointB : pointA;
                FlipSprite();
            }
        }

        void FlipSprite()
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        void HandleAttack()
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
            animator.SetTrigger("Attack");
            Attack();
            attackTimer = attackCooldown;
            }
        }

        void Attack()
        {
            
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Projectile projScript = projectile.GetComponent<Projectile>();
            projScript.Launch();
        }
    }

