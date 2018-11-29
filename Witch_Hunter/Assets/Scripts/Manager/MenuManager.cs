using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MenuManager : MonoBehaviour {
    #region Private Variable
    #endregion

    #region Public Variable
    public enum Menu
    {
        Inventory,
        Equipment,
        Combination,
        MiniMap,
        Option,
    }
    Menu m_Menu = Menu.Inventory;
    #endregion

    #region Private Method
    private void Start()
    {
    }
    private void Update()
    {
        switch(m_Menu)
        {
            case Menu.Inventory:
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
    #endregion

}
