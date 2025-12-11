using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance; // 플레이어 매니저 전역 변수

    public static PlayerManager Instance // 플레이어 매니저 인스턴스 접근용 프로퍼티
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("PlayerManager").AddComponent<PlayerManager>();
            }

            return instance;
        }
    }

    public XRPlayer Player { get; set; }  // XR용 플레이어 프로퍼티

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
        }
    }
}
