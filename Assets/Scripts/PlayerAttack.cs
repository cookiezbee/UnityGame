using TMPro;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject pistolModel;
    [SerializeField] GameObject batModel;

    [SerializeField] WeaponLogic pistolLogic;
    [SerializeField] WeaponLogic batLogic;

    [SerializeField] Animator animator;

    private int currentWeapon = 1;
    private bool isAiming = false;

    void Start() => EquipPistol();

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f) EquipPistol();
        else if (scroll < 0f) EquipBat();

        if (currentWeapon == 1)
        {
            if (Input.GetMouseButton(1))
            {
                isAiming = true;
                animator.SetBool("Aiming", true);

                if (Input.GetMouseButtonDown(0)) PerformAttack();
            }
            else
            {
                isAiming = false;
                animator.SetBool("Aiming", false);
            }
        }

        else if (currentWeapon == 2)
        {
            if (Input.GetMouseButtonDown(0)) PerformAttack();
        }
    }

    void EquipPistol()
    {
        currentWeapon = 1;
        pistolModel.SetActive(true);
        batModel.SetActive(false);
        animator.SetInteger("WeaponType", 1);

        isAiming = false;
        animator.SetBool("Aiming", false);
    }

    void EquipBat()
    {
        currentWeapon = 2;
        pistolModel.SetActive(false);
        batModel.SetActive(true);
        animator.SetInteger("WeaponType", 2);
    }

    void PerformAttack()
    {
        if (DialogueController.IsDialogueActive) return;

        animator.SetTrigger("Attack");

        if (currentWeapon == 1) pistolLogic.shot();
        else batLogic.shot();
    }
}
