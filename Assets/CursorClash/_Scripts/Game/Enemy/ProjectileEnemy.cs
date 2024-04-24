using UnityEngine;

namespace MichiTheDev
{
   public class ProjectileEnemy : Enemy
   {
      [SerializeField] protected Projectile _projectilePrefab;
      [SerializeField] protected float _projectileSpeed;
      
      protected override void Attack()
      {
         base.Attack();
         Projectile projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
         projectile.Shoot((PlayerCursor.Instance.transform.position - new Vector3(0.33f, 0.33f) - transform.position).normalized, _projectileSpeed);
      }
   }
}