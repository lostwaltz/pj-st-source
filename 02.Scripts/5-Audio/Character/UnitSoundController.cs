using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSoundController : MonoBehaviour
{
    private SoundHandler soundHandler;
    private bool isPlayingSelectSound = false;
    private Coroutine selectSoundCoroutine;
    private bool isInitialized = false;
    private PlayerUnit parentUnit;

    private void Start()
    {
        SoundHandler[] handlers = GetComponentsInChildren<SoundHandler>();
        foreach (var handler in handlers)
        {
            if (handler.CompareTag("Character"))
            {
                soundHandler = handler;
                break;
            }
        }

        parentUnit = GetComponentInParent<PlayerUnit>();
        if (parentUnit != null)
        {
            StartCoroutine(InitializeWhenReady());
        }
    }

    private IEnumerator InitializeWhenReady()
    {
        while (GameManager.Instance == null)
        {
            yield return null;
        }

        while (GameManager.Instance.Interaction == null)
        {
            yield return null;
        }

        // 이미 초기화되었거나 부모 유닛이 파괴된 경우 리턴
        if (isInitialized || parentUnit == null)
        {
            yield break;
        }

        // 이벤트 구독
        GameManager.Instance.Interaction.OnPlayerUnitSelected += OnUnitSelected;
        isInitialized = true;
    }

    private void OnDestroy()
    {
        // 코루틴 정리
        if (selectSoundCoroutine != null)
        {
            StopCoroutine(selectSoundCoroutine);
            selectSoundCoroutine = null;
            isPlayingSelectSound = false;
        }

        // 초기화되었고 GameManager가 존재하는 경우에만 이벤트 구독 해제 시도
        if (isInitialized && GameManager.Instance != null && GameManager.Instance.Interaction != null)
        {
            try
            {
                GameManager.Instance.Interaction.OnPlayerUnitSelected -= OnUnitSelected;
            }
            catch (System.Exception)
            {
                // 씬 전환 중이나 게임 종료 시 예외가 발생?
            }
        }
    }

    private void OnUnitSelected(PlayerUnit selectedUnit)
    {
        if (this == null || !gameObject.activeInHierarchy) return;

        var myPlayerUnit = GetComponentInParent<PlayerUnit>();
        // 부모 유닛이 없거나 파괴된 경우 이벤트 구독 해제
        if (myPlayerUnit == null)
        {
            if (GameManager.Instance != null && GameManager.Instance.Interaction != null)
            {
                GameManager.Instance.Interaction.OnPlayerUnitSelected -= OnUnitSelected;
            }
            return;
        }

        if (selectedUnit == myPlayerUnit)
        {
            PlaySelectSound();
        }
    }

    public void PlayFootstepSound()
    {
        if (soundHandler == null)
        {
            return;
        }

        soundHandler.PlaySoundFromContainer("footstep", transform.position);
    }

    public void PlayReloadSound()
    {
        if (soundHandler == null)
        {
            return;
        }

        soundHandler.PlaySoundFromContainer("reload", transform.position);
    }

    public void PlaySkill2Sound()
    {
        if (soundHandler == null)
        {
            return;
        }

        soundHandler.PlaySoundFromContainer("skill2", transform.position);
    }
    public void PlayDieSound()
    {
        if (soundHandler == null)
        {
            return;
        }

        soundHandler.PlaySoundFromContainer("die", transform.position);
    }
    public void PlayCartridgeSound()
    {
        if (soundHandler == null)
        {
            return;
        }

        soundHandler.PlaySoundFromContainer("cartridge", transform.position);
    }
    public void PlayJumpSound()
    {
        if (soundHandler == null)
        {
            return;
        }

        soundHandler.PlaySoundFromContainer("jump", transform.position);
    }
    public void PlayGunDropSound()
    {
        if (soundHandler == null)
        {
            return;
        }

        soundHandler.PlaySoundFromContainer("gundrop", transform.position);
    }
    public void PlayCollisionSound()
    {
        if (soundHandler == null)
        {
            return;
        }

        soundHandler.PlaySoundFromContainer("collision", transform.position);
    }

    public void PlaySelectSound()
    {
        if (soundHandler == null || isPlayingSelectSound)
        {
            return;
        }

        if (selectSoundCoroutine != null)
        {
            StopCoroutine(selectSoundCoroutine);
        }

        selectSoundCoroutine = StartCoroutine(PlaySelectSoundCoroutine());
    }

    private IEnumerator PlaySelectSoundCoroutine()
    {
        isPlayingSelectSound = true;
        soundHandler.PlaySoundFromContainer("select", transform.position);

        // 기본 대기 시간 설정 
        yield return new WaitForSeconds(1.5f);

        isPlayingSelectSound = false;
        selectSoundCoroutine = null;
    }

    private void OnDisable()
    {
        if (selectSoundCoroutine != null)
        {
            StopCoroutine(selectSoundCoroutine);
            selectSoundCoroutine = null;
            isPlayingSelectSound = false;
        }
    }

    private void OnApplicationQuit()
    {
        isPlayingSelectSound = false;
        selectSoundCoroutine = null;
    }
}



