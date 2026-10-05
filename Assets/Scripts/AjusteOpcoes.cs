using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AjusteOpcoes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Recuperação dos valores salvos em PlayerPrefs
        if (PlayerPrefs.HasKey("ModoControle") == false)
            PlayerPrefs.SetString("ModoControle", "Teclado+Mouse");

        if (PlayerPrefs.HasKey("Dificuldade") == false)
            PlayerPrefs.SetString("Dificuldade", "Normal");

        Geral.ModoDeControle = PlayerPrefs.GetString("ModoControle");
        Geral.Dificuldade = PlayerPrefs.GetString("Dificuldade");

    }

    public void AtualizarValores(bool toggleOn)
    {
        if (toggleOn)
        {
            GameObject toggleClicado = EventSystem.current.currentSelectedGameObject;

            switch (toggleClicado.name)
            {
                case "OpcaoMouse":
                    Geral.ModoDeControle = "Teclado+Mouse";
                    break;
                case "OpcaoTeclado":
                    Geral.ModoDeControle = "SoTeclado";
                    break;
                case "OpcaoControle":
                    Geral.ModoDeControle = "Controle";
                    break;
                case "OpcaoNormal":
                    Geral.Dificuldade = "Normal";
                    break;
                case "OpcaoAlta":
                    Geral.Dificuldade = "Alta";
                    break;
            }
            
            PlayerPrefs.SetString("ModoControle", Geral.ModoDeControle);
            PlayerPrefs.SetString("Dificuldade", Geral.Dificuldade);
        }
    }
}
