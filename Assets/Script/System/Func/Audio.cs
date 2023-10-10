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

    private AudioSource audioSource = null;

    public bool IsNowPlaying()
    {
        if (audioSource != null) return true;
        else return false;
    }
    public void NowPlayingStop()
    {
        audioSource.Stop();
    }
    public void IsLoop(bool value)
    {
        audioSource.loop = value;
    }

    // ��ȣ�� ���� ����� ����� ��� 
    public void AudioPlay(int idx)
    {
        audioSource.clip = audioClips[idx];
        audioSource.Play();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }
}
