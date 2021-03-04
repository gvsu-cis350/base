//public interface such that objects and classes can be assigned as damageable enteties
public interface IDamageable
{
    //initial method which dmagable entities extend so that all damage and rely on this base method
    void TakeDamage(float damage);
}