using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Plugins.InputHandler;

public class gameSafeController : MonoBehaviour
{
    [Header("Objects")]
    public GameObject SafeDoor;
    public GameObject SafeUI;
    public GameObject InteractTxt;
    public Button firstButton;
    public TMP_InputField SafeCodeInputField;
    public Game.Gameplay.gameLockerGunScreen SafeGun;

    bool safeDone;
    bool interactingWithSafe;
    bool playerInsideCollider;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        InteractTxt.GetComponent<TMP_Text>().text = "Interact";
        SafeDoor.transform.localRotation = Quaternion.Euler(0, 0, 0);

        ShowSafeUI(false);

        if (PropertiesClass.SafeOpen || bool.Parse(PlayerPrefs.GetString("SafeOpen", "false")))
            UnlockSafe(true);
    }

    private void Update()
    {
        InteractChecker();
    }

    private void InteractChecker()
    {
        if (playerInsideCollider && !safeDone)
        {
            InteractTxt.SetActive(true);
            if (InputHandler.GetButtonDown(InputHandler.eButtonCode.Interact))
            {
                InteractWithLock();
            }
        }
        else
        {
            InteractTxt.SetActive(false);
        }
    }

    private void InteractWithLock()
    {
        //ShowSafeUI(!interactingWithSafe);
        ShowSafeUI(true);
    }

    public void ShowSafeUI(bool Stats, bool DontUpdatePlayerMove = false)
    {
        ShowCursor(Stats);

        if (!DontUpdatePlayerMove)
            MovementPlayer.canMove = !Stats;
        
        interactingWithSafe = Stats;
        SafeUI.SetActive(Stats);

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        firstButton.Select();
    }

    private void ShowCursor(bool Stats)
    {
        Cursor.lockState = Stats ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = Stats ? true : false;
    }

    public void SafeCodeChanged(string safeCode)
    {
        char[] charArrayCode = safeCode.ToCharArray();

        SafeCodeInputField.text = "";

        if (charArrayCode.Length > 5)
        {
            for (int i = 0; i < 5; i++)
            {
                SafeCodeInputField.text += charArrayCode[i];
            }
        }
        else
        {
            foreach (char c in charArrayCode)
            {
                SafeCodeInputField.text += c;
            }
        }

        if (safeCode == "22123")
        {
            ShowSafeUI(false, true);
            UnlockSafe();
        }
    }

    public void WriteText(string TextToWrite)
    {
        SafeCodeInputField.text += TextToWrite;
        SafeCodeChanged(SafeCodeInputField.text);
    }

    public void DeleteText(bool AllText)
    {
        if (AllText)
            SafeCodeInputField.text = "";
        else
        {
            char[] charArrayCode = SafeCodeInputField.text.ToCharArray();
            SafeCodeInputField.text = "";
            for (int i = 0; i < charArrayCode.Length - 1; i++)
            {
                SafeCodeInputField.text += charArrayCode[i];
            }
        }

        SafeCodeChanged(SafeCodeInputField.text);
    }

    private void UnlockSafe(bool alreayOpened = false)
    {
        safeDone = true;
        PropertiesClass.SafeOpen = true;
        PlayerPrefs.SetString("SafeOpen", "true");
        SafeDoor.transform.localRotation = Quaternion.Euler(0, -40, 0);
        
        if (!alreayOpened)
            SafeGun.GoToScreen();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInsideCollider = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInsideCollider = false;
    }
}