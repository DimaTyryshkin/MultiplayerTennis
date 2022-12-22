using System;
using UnityEngine;
using UnityEngine.Networking;

public class PositionInterpolation : NetworkBehaviour
{
    NetworkTransform network;
    
    void Start()
    {
        network = GetComponent<NetworkTransform>();
        network.clientMoveCallback3D += OnMoveCallBack;
    }

    bool OnMoveCallBack(ref Vector3 position, ref Vector3 velocity, ref Quaternion rotation)
    {
        throw new NotImplementedException();
    }

    void Update()
    { 
    }
}