﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour, IWeapon
{
    public Vector3 projectileSpawn { get; set; }

    [SerializeField]
    protected GameObject projectilePrefab;

    protected GameObject currentProjectile;

    [SerializeField]
    protected float maxAngle, minAngle, rotateDegrees;

    [SerializeField]
    protected float maxShootPower;

    [SerializeField]
    protected float shootPowerIncrement;

    protected float shootPower;

    public void RotateUp()
    {
        if (this.transform.eulerAngles.z < maxAngle)
        {
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, this.transform.eulerAngles.z + rotateDegrees);
        }
    }

    public void RotateDown()
    {
        if (this.transform.eulerAngles.z > minAngle)
        {
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, this.transform.eulerAngles.z - rotateDegrees);
        }
    }

    public abstract void Fire();
}