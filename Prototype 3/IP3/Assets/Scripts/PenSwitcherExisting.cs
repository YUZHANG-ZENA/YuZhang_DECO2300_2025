using UnityEngine;

public class PenSwitcherExisting : MonoBehaviour
{
    public GameObject penRed;
    public GameObject penGreen;
    public GameObject penYellow;
    public GameObject eraser;

    void EnableOnly(GameObject target)
    {
        if (penRed)    penRed.SetActive(target == penRed);
        if (penGreen)  penGreen.SetActive(target == penGreen);
        if (penYellow) penYellow.SetActive(target == penYellow);
        if (eraser)    eraser.SetActive(target == eraser);
    }

    public void UseRed()    => EnableOnly(penRed);
    public void UseGreen()  => EnableOnly(penGreen);
    public void UseYellow() => EnableOnly(penYellow);
    public void UseEraser() => EnableOnly(eraser);
}

