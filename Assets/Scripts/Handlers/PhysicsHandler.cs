using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PhysicsHandler : MonoBehaviour
{
    internal static PhysicsHandler instance;

    internal Vector3 GlobalVelocity { get; private set; }
    private List<Vector3> allGlobalVelocities, allGlobalRecoveries;

    private void Awake()
    {
        instance = this;
        GlobalVelocity = Vector3.zero;
        allGlobalVelocities = new List<Vector3>();
        allGlobalRecoveries = new List<Vector3>();
    }

    private void FixedUpdate()
    {
        CheckGlobalVelocity();
    }

    private void CheckGlobalVelocity()
    {
        GlobalVelocity = Greenie.instance.LocalHitVelocity;
        for (int i = 0; i < allGlobalVelocities.Count; i++)
        {
            int signalFactor = allGlobalVelocities[i].x > 0 ? 1 : -1;

            GlobalVelocity += allGlobalVelocities[i];
            allGlobalVelocities[i] -= allGlobalRecoveries[i] * Time.fixedDeltaTime;

            if (signalFactor * allGlobalVelocities[i].x < 0)
            {
                allGlobalVelocities[i] = Vector3.zero;
                allGlobalVelocities.RemoveAt(i);
                allGlobalRecoveries.RemoveAt(i);
                i--;
            }
        }
    }

    internal void ResetGlobalVelocity()
    {
        allGlobalVelocities.Clear();
        allGlobalRecoveries.Clear();
        GlobalVelocity = Vector3.zero;
    }

    internal void AddGlobalVelocity(Vector3 velocity, Vector3 recovery)
    {
        allGlobalVelocities.Add(velocity);
        allGlobalRecoveries.Add(recovery);
    }

    internal void PushCharacter(Vector3 pushForce, Character character)
    {
        character.LocalHitVelocity += pushForce * Character.basePushForceFactor;
        character.OnPush();
    }
}