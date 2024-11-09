using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // 나뭇잎등의 레이어에 시차 효과(카메라의 반대쪽으로 해당 레이어를 옮김)를 추가하는 스크립트
    [SerializeField] private float parallaxOffset = -0.15f;

    private Camera cam;
    private Vector2 startPos;
    // =>: 람다식, 연산자 왼쪽이 파라미터, 연산자 오른쪽이 실행문장
    // 즉, travel을 파라미터로 하여 오른쪽 문장이 실행되고, 그 값을 travel에 저장한다..
    private Vector2 travel => (Vector2)cam.transform.position - startPos;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        startPos = transform.position; // startPos 초기화
    }

    private void FixedUpdate()
    {
        transform.position = startPos + travel * parallaxOffset;
        // 시작값에서 travel * parallaxOffset만큼을 더한값만큼 오브젝트를 이동시킨다.
    }
}
