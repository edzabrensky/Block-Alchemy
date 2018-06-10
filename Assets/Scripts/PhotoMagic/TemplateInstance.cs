using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TemplateInstance : MonoBehaviour
{
    private GameObject creation;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetThumbnail(Sprite visual)
    {
        sr.sprite = visual;
    }

    public void SetCreation(GameObject c)
    {
        creation = c;
    }

    public void Release()
    {
        Destroy(this.gameObject);
    }

    public void CreateObject()
    {
        Instantiate(creation, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}