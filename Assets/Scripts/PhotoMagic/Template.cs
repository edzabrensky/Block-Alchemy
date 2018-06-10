using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class Template : MonoBehaviour
{
    [SerializeField]
    private TemplateInstance instance;
    [SerializeField]
    private Sprite thumbnail;
    [SerializeField]
    private GameObject creation;
    private Image image;
    
    private void Awake()
    {
        this.image = GetComponent<Image>();
    }

    public void SetThumbnail(Sprite s)
    {
        thumbnail = s;
        this.image.sprite = s;
    }

    public void SetCreation(GameObject c)
    {
        creation = c;
    }

    public TemplateInstance CreateInstance()
    {
        // We want to move the object a bit forward of the menu
        Vector3 directionToMoveIn = (ServiceLocator.Instance.GetPlayerPosition() - transform.position).normalized;
        Vector3 newPosition = transform.position + (directionToMoveIn * 10);
        GameObject instanceCreated = Instantiate(instance.gameObject, newPosition, Quaternion.identity);
        TemplateInstance ti = instanceCreated.GetComponent<TemplateInstance>();
        ti.SetCreation(creation);
        ti.SetThumbnail(thumbnail);
        return ti;
    }
}