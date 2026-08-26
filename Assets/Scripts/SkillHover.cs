using UnityEngine;
using UnityEngine.EventSystems;

public class SkillHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SkillSlot skillSlot;

    private void Awake()
    {
        skillSlot = GetComponent<SkillSlot>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (skillSlot == null)
        {
            return;
        }

        if (skillSlot.skill == null)
        {
            return;
        }

        if (SkillTooltip.Instance == null)
        {
            return;
        }

        SkillTooltip.Instance.Show(skillSlot.skill);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (SkillTooltip.Instance != null)
            SkillTooltip.Instance.Hide();
    }
}