using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InputTextDoCriandoNovoJogo : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]private InputField input;
#pragma warning restore 0649    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Iniciar()
    {
        gameObject.SetActive(true);
    }

    public void CriandoJogo()
    {
        gameObject.SetActive(false);

        PropriedadesDeSave prop = new PropriedadesDeSave() { nome = input.text, ultimaJogada = System.DateTime.Now };
        //SaveDatesManager salvador = new SaveDatesManager();
        List<PropriedadesDeSave> lista = SaveDatesManager.s.SaveProps;//(List<PropriedadesDeSave>)(salvador.CarregarArquivo("criaturesGames.ori"));

        if (lista != null)
        {
            int maior = 0;

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].indiceDoSave > maior)
                    maior = lista[i].indiceDoSave;
            }

            prop.indiceDoSave = maior+1;
            lista.Add(prop);
        }
        else
            lista = new List<PropriedadesDeSave>() { prop };

        SaveDatesManager.s.SaveProps = lista;
        SaveDatesManager.Save();
        SaveDatesManager.s.IndiceDoJogoAtualSelecionado = prop.indiceDoSave;

        GlobalController.g.FadeV.IniciarFadeOutComAction(OnFadeOutComplete);
        //EventAgregator.AddListener(EventKey.fadeOutComplete, OnFadeOutComplete);  
    }

    void OnFadeOutComplete()
    {

        GlobalController.g.FadeV.IniciarFadeIn();
        SceneLoader.IniciarCarregamento(SaveDatesManager.s.IndiceDoJogoAtualSelecionado);
    }

    /*
    void IniciarCarregarCena(int indice)
    {
        gameObject.SetActive(false);
        GameObject G = new GameObject();
        SceneLoader loadScene = G.AddComponent<SceneLoader>();
        loadScene.CenaDoCarregamento(indice);
    }*/

    public void Voltar()
    {
        gameObject.SetActive(false);
        EventAgregator.Publish(EventKey.returnOfInputNameOfGame, null);
    }
}
