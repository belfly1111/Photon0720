using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] GameObject Door;
    public Light_Switch LS;
    public Shadow_Switch SS;

    private void Update()
    {
        if(LS.onSwitch && SS.onSwitch)
        {
            AudioSource a = GetComponent<AudioSource>(); 
            if (!a.isPlaying)
            {
                a.Play();
            }
            StartCoroutine(Destroyer());
        }
    }
    IEnumerator Destroyer()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
