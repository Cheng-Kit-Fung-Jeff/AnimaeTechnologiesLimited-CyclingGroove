using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_UIParticleGenerator : MonoBehaviour
{

    public Transform parent;
    public GameObject particle;
    public Sprite image;
    public List<Profile> directions;
    [System.Serializable]
    public struct Profile
    {
        public float angle;
        public float speed;
    }
    public float angleOffset;
    [Min(0)] public float maxSpeed;
    public Vector2 gravity;
    [Min(0)]public float life;
    
    public void Generate()
    {
        foreach (var d in directions)
        {
            Vector2 initVelocity
                = new(
                    d.speed * Mathf.Cos(Mathf.Deg2Rad * (d.angle + angleOffset)),
                    d.speed * Mathf.Sin(Mathf.Deg2Rad * (d.angle + angleOffset)));
            CKF_UIParticle newParticle = Instantiate(particle, parent).GetComponent<CKF_UIParticle>();
            if(image != null && newParticle.image != null)
                newParticle.image.sprite = image;
            newParticle.life.setTimer(life);
            newParticle.physics.velocity = initVelocity;
            newParticle.physics.maxVelocity = maxSpeed;
            newParticle.physics.gravity = gravity;
        }
    }
}
