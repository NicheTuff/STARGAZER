using UnityEngine;

namespace Assets.EntitySpace
{
    public abstract class Entity : MonoBehaviour
    {
        public abstract int MaxHealth { get; protected set; }
        public int Health { get; protected set; }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}