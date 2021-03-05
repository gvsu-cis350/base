/// <summary>
/// Public interface such that objects and classes can be assigned as damageable enteties
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Initial method which dmagable entities extend so that all damage and rely on this base method
    /// </summary>
    /// <param name="damage"></param>
    void TakeDamage(float damage);
}