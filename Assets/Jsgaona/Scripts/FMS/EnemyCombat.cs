
using UnityEngine;
namespace Jsgaona
{
public class EnemyCombat : MonoBehaviour
{

    [SerializeField] private Weapon weapon;
    [SerializeField] private string nameParameter = "IsAttacking";
    [SerializeField] private Animator animController;


    private float nextCheck = 1f;
    public bool IsAttacking => animController.GetBool(nameParameter);
    
    private void Awake()
    {
       
    }
    public void BasicAttack(string nameAnim)
    {
        if (!string.IsNullOrEmpty(nameAnim)) animController.CrossFade(nameAnim, 0.1f);
    }
    public bool CheckIntervalBasicAttack(float speedAttack)
    {
        if (Time.timeSinceLevelLoad < nextCheck) return false;
        nextCheck = Time.timeSinceLevelLoad + speedAttack; 
        return true;
    }

    public void UseWeapon()
    {
        if (weapon != null) weapon.Fire();
    }
}

}
