using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform shootPos;

    private float nextTimeToFire;

    public float fireRate = 15f;

    public IntVariable selectedSpellId;

    private Camera mainCamera;
    private Plane groundPlane;

    private Vector2 moveVec;

    private void Start()
    {
        mainCamera = Camera.main;
        groundPlane = new Plane(Vector3.up, transform.position);
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            if (selectedSpellId.Value > -1)
            {
                Vector3 mousePos = GetCursorWorldPosition();
                ClientSend.ShootProjectile(mousePos, selectedSpellId.Value);
                AbilitySelector.instance.StartCooldownForSelectedSpell();
            }
        }
        */
    }

    private void FixedUpdate()
    {
        SendInputToServer();
    }

    Vector3 GetCursorWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector3 point = Vector3.zero;
        if (groundPlane.Raycast(ray, out float distance))
        {
            point = ray.GetPoint(distance);
        }
        return new Vector3(point.x, transform.position.y, point.z);
    }

    #region Player Input
    public void OnMove(InputValue input)
    {
        moveVec = input.Get<Vector2>();
    }

    public void OnJump()
    {
        ClientSend.PlayerJump();
    }

    public void OnFirstSpell()
    {
        CastSpell(0);
    }

    public void OnSecondSpell()
    {
        CastSpell(1);
    }

    public void OnThirdSpell()
    {
        CastSpell(2);
    }

    public void OnFourthSpell()
    {
        CastSpell(3);
    }


    #endregion

    private void CastSpell(int spellPos)
    {
        int spellId = AbilitySelector.instance.GetSpellIdAtPos(spellPos);
        if (selectedSpellId.Value > -1)
        {
            ClientSend.ShootProjectile(GetCursorWorldPosition(), spellId);
            AbilitySelector.instance.StartCooldownForSpellWithPosition(spellPos);
        }
    }

    private void SendInputToServer()
    {
        float[] inputs = new float[]
        {
            moveVec.x,
            moveVec.y
        };

        ClientSend.PlayerMovement(inputs);
    }
}