using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [Header("Projectile Settings")]

    [SerializeField] public float speed = 8f;
    [SerializeField] public float lifeTime = 1;
    [SerializeField] public float gravity = 0; 
    [SerializeField] public float drag = 0; 
    [SerializeField] public int bounceCount = 0; 
    
    [HideInInspector] public Vector3 dir = Vector3.up;

    void Start()
    {
        dir = dir.normalized;    
    }

    void Update()
    {
        this.transform.position += speed * Time.deltaTime * dir;    
        lifeTime -= Time.deltaTime;

        if(lifeTime <= 0) GameObject.Destroy(this.gameObject);
    }

    public float GetSpeed() => speed;
    public void SetSpeed(float value) => speed = value;
    public void AddSpeed(float value) => speed += value;
    public void MulSpeed(float value) => speed *= value;

    public float GetLifeTime() => lifeTime;
    public void SetLifeTime(float value) => lifeTime = value;
    public void AddLifeTime(float value) => lifeTime += value;
    public void MulLifeTime(float value) => lifeTime *= value;

    public float GetGravity() => gravity;
    public void SetGravity(float value) => gravity = value;
    public void AddGravity(float value) => gravity += value;
    public void MulGravity(float value) => gravity *= value;

    public float GetDrag() => drag;
    public void SetDrag(float value) => drag = value;
    public void AddDrag(float value) => drag += value;
    public void MulDrag(float value) => drag *= value;

    public int GetBounceCount() => bounceCount;
    public void SetBounceCount(int value) => bounceCount = value;
    public void AddBounceCount(int value) => bounceCount += value;
}
