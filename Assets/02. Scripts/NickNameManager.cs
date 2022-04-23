using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class NickNameManager : MonoBehaviour
{
    public GameObject nickNameView;

    public Text nickNameText;
    public InputField inputField;

    public NotionManager notionManager;

    public PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        nickNameView.SetActive(false);
    }

    public void OpenNickName()
    {
        if (!nickNameView.activeSelf)
        {
            inputField.text = "";

            nickNameView.SetActive(true);
        }
        else
        {
            nickNameView.SetActive(false);
        }
    }

    public void CheckNickName()
    {
        if (playerDataBase.Gold >= 100)
        {
            string Check = Regex.Replace(inputField.text, @"[^a-zA-Z0-9��-�R]", "", RegexOptions.Singleline);
            Check = Regex.Replace(inputField.text, @"[^\w\.@-]", "", RegexOptions.Singleline);

            if (inputField.text.Equals(Check) == true)
            {
                string newNickName = ((inputField.text.Trim()).Replace(" ", ""));
                string oldNickName = GameStateManager.instance.NickName.Trim().Replace(" ", "");

                if (newNickName.Length > 2)
                {
                    if (!(newNickName.Equals(oldNickName)))
                    {
                        PlayfabManager.instance.UpdateDisplayName(newNickName, Success, Failure);
                    }
                    else
                    {
                        notionManager.UseNotion(NotionType.NickNameNotion1);
                        Debug.Log("�ߺ��� �г��� �Դϴ�.");
                    }
                }
                else
                {
                    notionManager.UseNotion(NotionType.NickNameNotion2);
                    Debug.Log("2���� �̻��̾�� �մϴ�.");
                }
            }
            else
            {
                notionManager.UseNotion(NotionType.NickNameNotion3);
                Debug.Log("Ư�����ڴ� ����� �� �����ϴ�.");
            }
        }
        else
        {
            notionManager.UseNotion(NotionType.NickNameNotion4);
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void Success()
    {
        Debug.Log("�г��� ���� ����!");

        notionManager.UseNotion(NotionType.NickNameNotion6);

        nickNameText.text = GameStateManager.instance.NickName;

        playerDataBase.Gold -= 100;

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, 100);

        nickNameView.SetActive(false);
    }

    public void Failure()
    {
        notionManager.UseNotion(NotionType.NickNameNotion5);
        Debug.Log("�̹� �����ϴ� �г��� �Դϴ�.");
    }
}
