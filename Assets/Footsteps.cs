using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    private CharacterController characterController;
    private AudioSource audioSource;
    public AudioClip footstepsSound;
    private FirstPersonController fs;
    void Start()
    {
        characterController = GameManager.Instance.player.GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        fs = GameManager.Instance.player.GetComponent<FirstPersonController>();

    }

    // Update is called once per frame
    void Update()
    {
        if(fs.Grounded)
        {
            if (characterController.velocity.x != 0 || characterController.velocity.z !=  0)
            {
                audioSource.clip = footstepsSound;
                if(!audioSource.isPlaying) audioSource.Play();
            }
            else
            {
                if (audioSource.isPlaying) audioSource.Stop();
            }
        }
        else
        {
            if (audioSource.isPlaying) audioSource.Stop();
        }

    }
}
