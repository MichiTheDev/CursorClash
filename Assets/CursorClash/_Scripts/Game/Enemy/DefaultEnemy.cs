namespace MichiTheDev
{
   public class DefaultEnemy : Enemy
   {
      protected override void Attack()
      {
         base.Attack();
         _attackCollider.enabled = true;
      }
   }
}