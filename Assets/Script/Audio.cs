using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
// ����� �ҽ� �����ؼ� �߰�
AudioSource audioSource = gameObject.AddComponent<AudioSource>();

// ��Ʈ: true�� ��� �Ҹ��� ���� ����
audioSource.mute = false;

// ����: true�� ��� �ݺ� ���
audioSource.loop = false;

// �ڵ� ���: true�� ��� �ڵ� ���
audioSource.playOnAwake = false;

// ����� ���
audioSource.Play();

// ����� ����
audioSource.Stop();
*/

public class Audio : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioClips;

    private AudioSource audioSource;

    public void AudioPlay(int idx)
    {
        audioSource.clip = audioClips[idx];
        audioSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
