using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState 
{
    /// <summary>
    /// CodeFlow: 
    /// Dau tien goi OnEnter
    /// Sau do se update lien tuc toi ham OnExecute
    /// Khi ma chuyen state khac thi se goi Exit cua state nay va enter state moi
    /// </summary>
    void OnEnter(Enemy enemy);
    void OnExecute(Enemy enemy);
    void OnExit(Enemy enemy);

}
