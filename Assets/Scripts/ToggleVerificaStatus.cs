using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleVerificaStatus : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        switch (name)
        {
            case "OpcaoMouse":
                GetComponent<Toggle>().isOn = Geral.ModoDeControle == "Teclado+Mouse";
                break;
            case "OpcaoTeclado":
                GetComponent<Toggle>().isOn = Geral.ModoDeControle == "SoTeclado";
                break;
            case "OpcaoControle":
                GetComponent<Toggle>().isOn = Geral.ModoDeControle == "Controle";
                break;
            case "OpcaoNormal":
                GetComponent<Toggle>().isOn = Geral.Dificuldade == "Normal";
                break;
            case "OpcaoAlta":
                GetComponent<Toggle>().isOn = Geral.Dificuldade == "Alta";
                break;
        }

    }
}
