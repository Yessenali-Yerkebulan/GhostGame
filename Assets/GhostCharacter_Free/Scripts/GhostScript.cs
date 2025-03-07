using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sample {
public class GhostScript : MonoBehaviour
{
    private Animator Anim;
    private CharacterController characterController;
    private Vector3 MoveDirection = Vector3.zero;
    // Cache hash values
    private static readonly int IdleState = Animator.StringToHash("Base Layer.idle");
    private static readonly int MoveState = Animator.StringToHash("Base Layer.move");
    private static readonly int SurprisedState = Animator.StringToHash("Base Layer.surprised");
    private static readonly int AttackState = Animator.StringToHash("Base Layer.attack_shift");
    private static readonly int DissolveState = Animator.StringToHash("Base Layer.dissolve");
    private static readonly int AttackTag = Animator.StringToHash("Attack");
    // dissolve
    [SerializeField] private SkinnedMeshRenderer[] MeshR;
    private float Dissolve_value = 1;
    private bool DissolveFlg = false;
    private const int maxHP = 3;
    private bool isDead;
    [SerializeField] Transform respawn;

    // moving speed
    [SerializeField] private float Speed = 4;

    void Start()
    {
        Anim = this.GetComponent<Animator>();
        characterController = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        STATUS();
        GRAVITY();
        // this character status
        if(!PlayerStatus.ContainsValue( true ))
        {
            MOVE();
        }
        else if(PlayerStatus.ContainsValue( true ))
        {
            int status_name = 0;
            foreach(var i in PlayerStatus)
            {
                if(i.Value == true)
                {
                    status_name = i.Key;
                    break;
                }
            }
            if(status_name == Dissolve)
            {
                PlayerDissolve();
            }
            else if(status_name == Attack)
            {
            }
            else if(status_name == Surprised)
            {
                // nothing method
            }
        }
        // Dissolve
        if(isDead && !DissolveFlg)
        {
            Anim.CrossFade(DissolveState, 0.1f, 0, 0);
            DissolveFlg = true;
        }
        // processing at respawn
        else if(!isDead && DissolveFlg)
        {
            DissolveFlg = false;
        }
    }

    //---------------------------------------------------------------------
    // character status
    //---------------------------------------------------------------------
    private const int Dissolve = 1;
    private const int Attack = 2;
    private const int Surprised = 3;
    private Dictionary<int, bool> PlayerStatus = new Dictionary<int, bool>
    {
        {Dissolve, false },
        {Attack, false },
        {Surprised, false },
    };
    //------------------------------
    private void STATUS ()
    {
        // during dissolve
        if(DissolveFlg && isDead)
        {
            PlayerStatus[Dissolve] = true;
        }
        else if(!DissolveFlg)
        {
            PlayerStatus[Dissolve] = false;
        }
        // during attacking
        if(Anim.GetCurrentAnimatorStateInfo(0).tagHash == AttackTag)
        {
            PlayerStatus[Attack] = true;
        }
        else if(Anim.GetCurrentAnimatorStateInfo(0).tagHash != AttackTag)
        {
            PlayerStatus[Attack] = false;
        }
        // during damaging
        if(Anim.GetCurrentAnimatorStateInfo(0).fullPathHash == SurprisedState)
        {
            PlayerStatus[Surprised] = true;
        }
        else if(Anim.GetCurrentAnimatorStateInfo(0).fullPathHash != SurprisedState)
        {
            PlayerStatus[Surprised] = false;
        }
    }
    // dissolve shading
    private void PlayerDissolve ()
    {
        Dissolve_value -= Time.deltaTime;
        for(int i = 0; i < MeshR.Length; i++)
        {
            MeshR[i].material.SetFloat("_Dissolve", Dissolve_value);
        }
        if(Dissolve_value <= 0)
        {
            characterController.enabled = false;
            Respawn();
        }
    }
    // play a animation of Attack
    private void PlayerAttack ()
    {
        Anim.CrossFade(AttackState,0.1f,0,0); 
    }
    //---------------------------------------------------------------------
    // gravity for fall of this character
    //---------------------------------------------------------------------
    private void GRAVITY ()
    {
        if(characterController.enabled)
        {
            if(CheckGrounded())
            {
                if(MoveDirection.y < -0.1f)
                {
                    MoveDirection.y = -0.1f;
                }
            }
            MoveDirection.y -= 0.1f;
            characterController.Move(MoveDirection * Time.deltaTime);
        }
    }
    //---------------------------------------------------------------------
    // whether it is grounded
    //---------------------------------------------------------------------
    private bool CheckGrounded()
    {
        if (characterController.isGrounded && characterController.enabled)
        {
            return true;
        }
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
        float range = 0.2f;
        return Physics.Raycast(ray, range);
    }
    //---------------------------------------------------------------------
    // for slime moving
    //---------------------------------------------------------------------
    private void MOVE ()
    {
        // velocity
        if(Anim.GetCurrentAnimatorStateInfo(0).fullPathHash == MoveState)
        {
            if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                MOVE_Velocity(new Vector3(0, 0, -Speed), new Vector3(0, 180, 0));
            }
            else if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                MOVE_Velocity(new Vector3(0, 0, Speed), new Vector3(0, 0, 0));
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                MOVE_Velocity(new Vector3(Speed, 0, 0), new Vector3(0, 90, 0));
            }
            else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                MOVE_Velocity(new Vector3(-Speed, 0, 0), new Vector3(0, 270, 0));
            }
        }
        KEY_DOWN();
        KEY_UP();
    }
    //---------------------------------------------------------------------
    // value for moving
    //---------------------------------------------------------------------
    private void MOVE_Velocity (Vector3 velocity, Vector3 rot)
    {
        MoveDirection = new Vector3 (velocity.x, MoveDirection.y, velocity.z);
        if(characterController.enabled)
        {
            characterController.Move(MoveDirection * Time.deltaTime);
        }
        MoveDirection.x = 0;
        MoveDirection.z = 0;
        this.transform.rotation = Quaternion.Euler(rot);
    }
    //---------------------------------------------------------------------
    // whether arrow key is key down
    //---------------------------------------------------------------------
    private void KEY_DOWN ()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Anim.CrossFade(MoveState, 0.1f, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Anim.CrossFade(MoveState, 0.1f, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Anim.CrossFade(MoveState, 0.1f, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Anim.CrossFade(MoveState, 0.1f, 0, 0);
        }
    }
    //---------------------------------------------------------------------
    // whether arrow key is key up
    //---------------------------------------------------------------------
    private void KEY_UP ()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if(!Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                Anim.CrossFade(IdleState, 0.1f, 0, 0);
            }
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if(!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                Anim.CrossFade(IdleState, 0.1f, 0, 0);
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if(!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                Anim.CrossFade(IdleState, 0.1f, 0, 0);
            }
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if(!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                Anim.CrossFade(IdleState, 0.1f, 0, 0);
            }
        }
    }
    //---------------------------------------------------------------------
    // damage
    //---------------------------------------------------------------------
    public void Damage ()
    {
            // Damaged by outside field.
        Anim.CrossFade(SurprisedState, 0.1f, 0, 0);
        if(!isDead)
            {
                DataContainer._deaths++;
            }
        isDead = true;
    }
    //---------------------------------------------------------------------
    // respawn
    //---------------------------------------------------------------------
    private void Respawn ()
    {
            // player HP
            isDead = false;
            
            characterController.enabled = false;
            this.transform.position = respawn.position; // player position
            this.transform.rotation = respawn.rotation; // player facing
            characterController.enabled = true;
            
            // reset Dissolve
            Dissolve_value = 1;
            for(int i = 0; i < MeshR.Length; i++)
            {
                MeshR[i].material.SetFloat("_Dissolve", Dissolve_value);
            }
            // reset animation
            Anim.CrossFade(IdleState, 0.1f, 0, 0);
    }


    public void LockInput()
        {
            characterController.enabled = false;
        }
}
}