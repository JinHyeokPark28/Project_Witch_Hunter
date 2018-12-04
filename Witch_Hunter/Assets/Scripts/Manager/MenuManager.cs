using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MenuManager : MonoBehaviour {
    #region Private Variable
    private static MenuManager _MenuMgr;
    #endregion

    #region Public Variable
    public enum Menu
    {
        NONSTOP,
        Inventory,
        Equipment,
        Combination,
        MiniMap,
        Option,
    }
    Menu m_Menu = Menu.Inventory;
    #endregion

    #region Private Method
    //private void Awake()
    //{
    //    if (_MenuMgr == null)
    //        _MenuMgr = this;
    //    else Destroy(this);

    //    DontDestroyOnLoad(this);
    //}

    private void Start()
    {
        GameManager.GetGameManager._MenuManager = this;
    }
    private void Update()
    {
        CheckMenu();
    }
    private void CheckMenu()
    {
        switch (m_Menu)
        {
            case Menu.Inventory:
                if (Input.GetKeyDown(KeyCode.I))
                {
                    Inventory.instance.activated = true;
                }
                else if(Input.GetKeyDown(KeyCode.I))
                {
                    Inventory.instance.activated = false;
                }
                break;
            case Menu.Equipment:
                break;
            case Menu.Combination:
                break;
            case Menu.MiniMap:
                break;
            case Menu.Option:
                break;

        }
    }
    #endregion

    #region Public Method
    public static MenuManager GetMenuMgr
    {
        get { return _MenuMgr; }
    }
    #endregion

}
