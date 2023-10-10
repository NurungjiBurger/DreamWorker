using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Map : MonoBehaviour
{
    [SerializeField]
    private List<float> monsterPosition = new List<float>();
    [SerializeField]
    private List<float> portalPosition = new List<float>();

    public List<float> SafeMonsterPosition { get { return monsterPosition; } }
    public List<float> SafePortalPosition { get { return portalPosition; } }

    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {

    }

    private void Update()
    {

    }

}
