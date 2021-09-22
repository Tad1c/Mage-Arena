using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform shootPos;
    public float fireRate = 15f;
    public IntVariable selectedSpellId;

    private Plane groundPlane;
    private Vector2 moveVec;

    private void Start()
    {
        groundPlane = new Plane(Vector3.up, transform.position);
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
        var spell = AbilitySelector.instance.GetSpellAtPosition(spellPos);

        if (spell == null) return;

        int spellId = spell.spellData.id;

        if (!spell.isInCooldown)
        {
            ClientSend.ShootProjectile(GetCursorWorldPosition(), spellId);
            spell.StartCooldown();
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