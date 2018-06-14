using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TakePhoto : MonoBehaviour
{
    [SerializeField]
    private RenderTexture settings;
    private RenderTexture renderTexture;
    private Camera photoCamera;
    public Sprite Photo { get; private set; }

    private void Awake()
    {
        this.photoCamera = GetComponent<Camera>();
        this.renderTexture = (settings == null) ? new RenderTexture(256, 256, 0) : new RenderTexture(settings);
    }

    private void Start()
    {
        this.photoCamera.targetTexture = this.renderTexture;
        photoCamera.enabled = false;
    }
    
    // Takes photo, use yield in a sequential coroutine to access photo
    public IEnumerator CapturePhoto(Transform target)
    {
        this.photoCamera.enabled = true;
        transform.LookAt(target.position);
        yield return new WaitForEndOfFrame();
        RenderTexture.active = this.renderTexture;
        Texture2D texture2D = new Texture2D(this.renderTexture.width, this.renderTexture.height);
        Rect dimensions = new Rect(0, 0, this.renderTexture.width, this.renderTexture.height);
        texture2D.ReadPixels(dimensions, 0, 0);
        texture2D.Apply();
        RenderTexture.active = null;
        Photo = Sprite.Create(texture2D, dimensions, new Vector2());
    }
}