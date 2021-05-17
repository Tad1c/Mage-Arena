using ScriptableObjectArchitecture;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGroundArrow : MonoBehaviour
{

    public LineRenderer lineRenderer;
    public IntVariable selectedSpellId;

    private Plane groundPlane;
    private Camera mainCamera;
    private Spell spell;

    private void Start()
    {
        mainCamera = Camera.main;
        selectedSpellId.AddListener(SpellSelected);
        groundPlane = new Plane(Vector3.up, transform.position);
        lineRenderer.positionCount = 2;
    }

    void SpellSelected()
    {
        if (selectedSpellId.Value > -1)
        {
            lineRenderer.enabled = true;
            spell = SpellDatabase.instance.GetSpellById(selectedSpellId.Value);
        }
        else
        {
            spell = null;
            lineRenderer.enabled = false;
        }
    }

    private void Update()
    {
        if (spell == null) return;

        var mousePosition = GetCursorWorldPosition();
        float distance = Vector3.Distance(transform.position, mousePosition);

        Vector3 vect = transform.position - mousePosition;
        vect = vect.normalized;
        vect *= (distance - spell.maxRange);
        mousePosition += vect;

        Vector3 toPos = new Vector3(mousePosition.x, transform.position.y, mousePosition.z);

        DrawLine(transform.position, toPos);
    }

    Vector3 GetCursorWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 point = Vector3.zero;
        if (groundPlane.Raycast(ray, out float distance))
        {
            point = ray.GetPoint(distance);
        }
        return new Vector3(point.x, transform.position.y, point.z);
    }

    public void DrawLine(Vector3 from, Vector3 to)
    {
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
    }

}
