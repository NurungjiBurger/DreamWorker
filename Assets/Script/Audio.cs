using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
// 오디오 소스 생성해서 추가
AudioSource audioSource = gameObject.AddComponent<AudioSource>();

// 뮤트: true일 경우 소리가 나지 않음
audioSource.mute = false;

// 루핑: true일 경우 반복 재생
audioSource.loop = false;

// 자동 재생: true일 경우 자동 재생
audioSource.playOnAwake = false;

// 오디오 재생
audioSource.Play();

// 오디오 정지
audioSource.Stop();
*/

public class Audio : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioClips;

    private AudioSource audioSource;

    // 번호에 따른 저장된 오디오 재생 
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
