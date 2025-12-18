using TMPro;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject pistolModel;
    [SerializeField] GameObject batModel;

    [SerializeField] Gun pistolLogic;
    [SerializeField] MeleeWeapon batLogic;

    [SerializeField] Animator animator;

    private Weapon activeWeapon;

    private int currentWeapon = 0;
    private bool isAiming = false;

    public AudioSource audioSource;
    public AudioClip pistolEquipSound;
    public AudioClip batEquipSound;

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        EquipPistol();
    }

    void Update()
    {
        if (DialogueController.IsDialogueActive || !PlayerMovement.PlayerHasMoved) return;

        if (!isAiming)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (Mathf.Abs(scroll) > 0f)
            {
                if (currentWeapon == 1) EquipBat();
                else EquipPistol();
            }
        }

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
        if (currentWeapon == 1) return;

        if (currentWeapon != 0 && audioSource != null && pistolEquipSound != null) audioSource.PlayOneShot(pistolEquipSound);

        currentWeapon = 1;
        pistolModel.SetActive(true);
        batModel.SetActive(false);

        activeWeapon = pistolLogic;

        animator.SetInteger("WeaponType", 1);
        isAiming = false;
        animator.SetBool("Aiming", false);
    }

    void EquipBat()
    {
        if (currentWeapon == 2) return;

        if (currentWeapon != 0 && audioSource != null && batEquipSound != null) audioSource.PlayOneShot(batEquipSound);

        currentWeapon = 2;
        pistolModel.SetActive(false);
        batModel.SetActive(true);

        activeWeapon = batLogic;

        animator.SetInteger("WeaponType", 2);
    }

    void PerformAttack()
    {
        if (DialogueController.IsDialogueActive) return;

        if (Time.time >= activeWeapon.nextAttackTime)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");

            activeWeapon.TryAttack();
        }
    }

    public void OnAnimationShootEvent()
    {
        if (activeWeapon != null) activeWeapon.OnAnimationEventTriggered();
    }
}
