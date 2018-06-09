using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTemplate : MonoBehaviour
{
    [SerializeField]
    private TakePhoto cam;
    [SerializeField]
    private Transform scrollContent;
    [SerializeField]
    private Template template;
    
    public void CreateTemplate(GameObject creation)
    {
        StartCoroutine(CreateTemplateCoroutine(creation));
    }

    private IEnumerator CreateTemplateCoroutine(GameObject creation)
    {
        GameObject createdTemplate = Instantiate(template.gameObject, scrollContent);
        Template t = createdTemplate.GetComponent<Template>();
        t.SetCreation(creation);
        yield return cam.CapturePhoto(creation.transform);
        t.SetThumbnail(cam.Photo);
    }
}