using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class AtomSpawner : MonoBehaviour
{
    [Header("Atom Prefabs")]
    public GameObject atomH_Prefab;
    public GameObject atomO_Prefab;
    public GameObject atomC_Prefab;
    public GameObject atomN_Prefab;

    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public float spawnOffset = 0.2f;

    [Header("Menu Reference")]
    public GameObject spawnMenu;
    public float animationDuration = 0.3f;

    private bool _menuVisible = false;

    void Start()
    {
        if (spawnMenu != null)
            spawnMenu.SetActive(false);
    }

    public void SpawnH() => SpawnAtom(atomH_Prefab);
    public void SpawnO() => SpawnAtom(atomO_Prefab);
    public void SpawnC() => SpawnAtom(atomC_Prefab);
    public void SpawnN() => SpawnAtom(atomN_Prefab);

    void SpawnAtom(GameObject prefab)
    {
        if (prefab == null) return;
        Vector3 pos = spawnPoint != null
            ? spawnPoint.position + spawnPoint.forward * spawnOffset
            : transform.position + Vector3.forward * spawnOffset;
        Instantiate(prefab, pos, Quaternion.identity);
        HideMenu();
    }


    public void ToggleMenu()
    {
        if (spawnMenu == null) return;
        spawnMenu.transform.DOKill();
        if (spawnMenu.activeSelf)
        {
            HideMenu();
        }
        else
        {
            ShowMenu();
        }
    }

    private void ShowMenu()
    {
        CanvasGroup cg = spawnMenu.GetComponent<CanvasGroup>();
        if (cg != null) cg.alpha = 0;
        spawnMenu.transform.localScale = Vector3.zero;
        spawnMenu.SetActive(true);
        spawnMenu.transform.DOScale(new Vector3(0.001f, 0.001f, 0.001f), animationDuration).SetEase(Ease.OutBack);
        if (cg != null) cg.DOFade(1, animationDuration);
    }

    private void HideMenu()
    {
        CanvasGroup cg = spawnMenu.GetComponent<CanvasGroup>();
        spawnMenu.transform.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack);
        if (cg != null)
        {
            cg.DOFade(0, animationDuration).OnComplete(() => spawnMenu.SetActive(false));
        }
        else
        {
            spawnMenu.transform.DOScale(Vector3.zero, animationDuration).OnComplete(() => spawnMenu.SetActive(false));
        }
    }
}