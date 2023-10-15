using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerParams", menuName = "Pro Platformer/Player Param", order = 1 )]
    public class PlayerParams : ScriptableObject
    {
        public float MarioSpeed = 10;
        public float MarioMaxSpeed = 20;
        public float MarioUpSpeed = 20;
        public float deathImpulse = 20;
        public float StarmanTime = 10;
    public float flickerInterval = 0.1f;

    public Vector3 BoxSize;
    public Vector3 BigBoxSize;
    public float MaxDistance;
    public LayerMask LayerMask; 

/*    private Action reloadCallback;
        public void SetReloadCallback(Action onReload)
        {
            this.reloadCallback = onReload;
        }*/

        public void OnValidate()
        {
            ReloadParams();
        }


        public void ReloadParams()
        {
            Debug.Log("=======更新所有Player配置参数");
            Constants.MarioSpeed = MarioSpeed;
            Constants.MarioMaxSpeed = MarioMaxSpeed;
            Constants.MarioUpSpeed = MarioUpSpeed;  
            Constants.BoxSize = BoxSize;
            Constants.MaxDistance = MaxDistance;
            Constants.LayerMask = LayerMask;
        Constants.deathImpulse = deathImpulse;
        Constants.StarmanTime = StarmanTime;     
        Constants.BigBoxSize = BigBoxSize;
        Constants.flickerInterval = flickerInterval;
        //reloadCallback?.Invoke();
        }
    }
