using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : IObjectFactory<Projectile>
{
    private Projectile projectilePrefab;
    public ProjectileFactory(Projectile projectilePrefab)
    {
        this.projectilePrefab = projectilePrefab;
    }

    public Projectile CreateObject()
    {
        return GameObject.Instantiate(projectilePrefab);
    }
}
