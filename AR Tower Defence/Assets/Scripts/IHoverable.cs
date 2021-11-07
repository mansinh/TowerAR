using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoverable 
{
    void OnHoverEnter();
    void OnHoverStay();
    void OnHoverLeave();
    ISelectable GetSelectable();
}
