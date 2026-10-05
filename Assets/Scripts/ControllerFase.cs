using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ControllerFase : MonoBehaviour
{
    // Instância atual do script
    public static ControllerFase instance;

    // Estrutura de composição das fases
    public int[] TipoPaciente;
    public string[] FalaPaciente;
    public string[] FalaDoutor;
    public string[] FalaFinal;
    public float[] TempoMaxTratamento;

    public Vector3[] PosicaoMonstrosAzuis;
    public Vector3[] PosicaoMonstrosVerdes;
    public Vector3[] PosicaoMonstrosRoxos;
    public Vector3[] PosicaoMonstrosAmarelos;
    public Vector3[] PosicaoMonstrosVermelhos;

    public Vector3[] PosicaoObstaculos;
    public Vector3[] DimensaoObstaculos;
    public Vector2[] PosicaoInicialPilulas;

    // Informações sobre a fase corrente
    internal int faseCorrente;
    internal float tempoRestante;
    internal string pilulaMaoEsquerda, pilulaMaoDireita;
    internal int qtdMonstrosAzuisRestantes, qtdMonstrosVerdesRestantes, qtdMonstrosRoxosRestantes, 
        qtdMonstrosAmarelosRestantes, qtdMonstrosVermelhosRestantes;
    internal string etapaAtual;
    internal string modoDeJogoAnterior = "Início"; // Utilizado para pausar o game
    internal bool statusBarraInferiorAoPausar = false;

    // Elementos da cena (Canvas + Laboratório)
    public GameObject doutorCenario, doutorConversa, doutorTratamento, doutorFelicidade, doutorTristeza;
    public GameObject pacienteCenario, pacienteNoLeito, coracaoLeito;
    public GameObject telaPause, telaVitoria, telaDerrota;
    public GameObject visualizacaoLabirinto, barraInfoAcoes;
    public GameObject objetoColliderCanvas;
    public Camera[] camerasTratamento;
    public Camera camPosicaoInicialPaciente, camDialogoPacienteDoutor, camPacienteIndoAoLeito, camMenuInicial;
    public AudioSource fundoSonoroConversaPaciente, fundoSonoroAcao, efeitoSonoroBom, efeitoSonoroRuim, somVitoria, somDerrota;
    public Image coracaoRecordeRef;

    public Text textoTempoRestante, textoMensagemMomento;
    public Text mensagemPapelEsquerda, mensagemPapelDireita;

    public MontaLabirinto controladorLabirinto;

    // Elementos de referência adicionais para construção das fases
    public Material[] estilosVisuaisPacientes;
    public Vector3 posicaoInicialPaciente, posicaoDialogoPaciente, posicaoPacienteIndoAoLeito;
    public Vector3 rotacaoInicialPaciente, rotacaoDialogoPaciente, rotacaoPacienteLeito;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Geral.ModoDeJogoCorrente = "Menu";
        faseCorrente = -1;

        VerificacaoInicialRecorde();
    }

    // Update is called once per frame
    void Update()
    {
        // Controle do tempo
        if (Geral.ModoDeJogoCorrente == "BuscaMedicamentos" || Geral.ModoDeJogoCorrente == "Labirinto")
        {
            tempoRestante -= Time.deltaTime;
            barraInfoAcoes.SetActive(true);
            textoTempoRestante.text = Mathf.FloorToInt(tempoRestante / 60).ToString("00") + ":" + Mathf.FloorToInt(tempoRestante % 60).ToString("00");

            if (tempoRestante < 0)
            {
                Geral.ModoDeJogoCorrente = "Perdeu";
                tempoRestante = 0; 
                barraInfoAcoes.SetActive(false);
                controladorLabirinto.gameObject.SetActive(false);
                visualizacaoLabirinto.SetActive(false);
                FindObjectOfType<ConcedePilula>().barraInferior.gameObject.SetActive(false);
                FindObjectOfType<ControllerMov_Tank>(true).enabled = false;
                telaDerrota.SetActive(true);
                doutorTristeza.SetActive(true);
                telaDerrota.GetComponentInChildren<Button>().Select();
                somDerrota.Play();

                // Reexibe a seta do mouse, caso aplicável
                if (Geral.ModoDeControle == "Teclado+Mouse")
                    Cursor.lockState = CursorLockMode.None;
            }
            else if ((Geral.ModoDeControle.Contains("Teclado") && Input.GetKeyDown(KeyCode.Escape)) || (Geral.ModoDeControle == "Controle" && Input.GetButtonDown("Xbox_Start")))
                Pausar();
        }
        else if (Geral.ModoDeJogoCorrente == "Pausado")
        {
            Time.timeScale = 0;
            barraInfoAcoes.SetActive(false);

            if ((Geral.ModoDeControle.Contains("Teclado") && Input.GetKeyDown(KeyCode.Escape)) || (Geral.ModoDeControle == "Controle" && Input.GetButtonDown("Xbox_Start")))
                Pausar();
        }
    }

    public void Pausar()
    {
        if (modoDeJogoAnterior == "Início" || modoDeJogoAnterior == "Pausado")
        {
            modoDeJogoAnterior = Geral.ModoDeJogoCorrente;
            Geral.ModoDeJogoCorrente = "Pausado";
            telaPause.SetActive(true);
            telaPause.GetComponentInChildren<Button>().Select();

            statusBarraInferiorAoPausar = FindObjectOfType<ConcedePilula>().barraInferior.gameObject.activeSelf;
            FindObjectOfType<ConcedePilula>().barraInferior.gameObject.SetActive(false);

            // Reexibe a seta do mouse, caso aplicável
            if (Geral.ModoDeControle == "Teclado+Mouse")
                Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Geral.ModoDeJogoCorrente = modoDeJogoAnterior;
            modoDeJogoAnterior = "Pausado";
            telaPause.SetActive(false);
            Time.timeScale = 1;

            FindObjectOfType<ConcedePilula>().barraInferior.gameObject.SetActive(statusBarraInferiorAoPausar);

            // Oculta a seta do mouse, caso aplicável
            if (Geral.ModoDeControle == "Teclado+Mouse")
                Cursor.lockState = CursorLockMode.Locked;
        }            
    }

    public void AtenderPaciente()
    {
        // Ativações e desativações
        doutorCenario.SetActive(false);
        doutorTratamento.SetActive(false);
        doutorFelicidade.SetActive(false);
        doutorTristeza.SetActive(false);
        pacienteNoLeito.SetActive(false);
        mensagemPapelEsquerda.transform.parent.gameObject.SetActive(false);
        mensagemPapelDireita.transform.parent.gameObject.SetActive(false);
        camDialogoPacienteDoutor.gameObject.SetActive(false);
        camMenuInicial.gameObject.SetActive(false);
        camPosicaoInicialPaciente.gameObject.SetActive(true);
        coracaoLeito.SetActive(false);

        // Alinhar rotação do leito e do labirinto
        foreach (ControllerMov_Labirinto labirinto in FindObjectsOfType<ControllerMov_Labirinto>(true))
        {
            labirinto.transform.eulerAngles = Vector3.zero;
            labirinto.rotacaoCalculada = Vector3.zero;
        }
        
        // Iniciar trilha sonora de conversa com o paciente
        foreach (AudioSource audio in FindObjectsOfType<AudioSource>())
            audio.Stop();
        fundoSonoroConversaPaciente.Play();

        // Troca a textura dos personagens, para corresponder ao estilo escolhido
        Material[] mat = new Material[1];
        mat[0] = estilosVisuaisPacientes[TipoPaciente[faseCorrente]];

        foreach (SkinnedMeshRenderer texturaMeshPacCenario in pacienteCenario.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            texturaMeshPacCenario.materials = mat;
        foreach (SkinnedMeshRenderer texturaMeshPacLeito in pacienteNoLeito.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            texturaMeshPacLeito.materials = mat;

        //Ativa o paciente e o doutor da primeira cutscene
        pacienteCenario.transform.localPosition = posicaoInicialPaciente;
        pacienteCenario.transform.localEulerAngles = rotacaoInicialPaciente;

        doutorConversa.SetActive(true);
        pacienteCenario.SetActive(true);

        // Caminhada inicial do paciente em direção ao médico
        etapaAtual = "PacienteCaminhadaInicial";
        StartCoroutine(caminhadaInicialPaciente());
    }

    IEnumerator caminhadaInicialPaciente()
    {
        // Aguardar 1 segundo e começar a caminhada
        pacienteCenario.GetComponent<Animator>().SetBool("caminharPeloCenario", false);
        yield return new WaitForSeconds(1f);
        pacienteCenario.GetComponent<Animator>().SetBool("caminharPeloCenario", true);

        // Interromper caminhada quando paciente chegar bem perto de posicaoDialogoPaciente
        while (Vector3.Distance(pacienteCenario.transform.position, posicaoDialogoPaciente)>0.25f)
        {
            pacienteCenario.transform.LookAt(posicaoDialogoPaciente);
            yield return new WaitForEndOfFrame();
        }

        // Rotacionar paciente para conversar com o doutor
        pacienteCenario.transform.position = posicaoDialogoPaciente;
        pacienteCenario.GetComponent<Animator>().SetBool("caminharPeloCenario", false);
        pacienteCenario.transform.LookAt(doutorConversa.transform);
        
        // Montar mensagem na tela, fala inicial do paciente
        yield return new WaitForSeconds(1f);
        MostrarMensagemPaciente();
    }

    public void MostrarMensagemPaciente()
    {
        // Troca de câmera
        camDialogoPacienteDoutor.gameObject.SetActive(true);
        camPosicaoInicialPaciente.gameObject.SetActive(false);

        // Ativa a mensagem e exibe na tela
        mensagemPapelEsquerda.transform.parent.gameObject.SetActive(true);
        mensagemPapelEsquerda.text = FalaPaciente[faseCorrente];

        objetoColliderCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(objetoColliderCanvas);
        etapaAtual = "DialogoPacienteMedico";
    }

    public void MostrarMensagemMedico()
    {
        // Troca de câmera
        camDialogoPacienteDoutor.gameObject.SetActive(false);
        camPacienteIndoAoLeito.gameObject.SetActive(true);

        // Ativa a mensagem e exibe na tela
        mensagemPapelEsquerda.transform.parent.gameObject.SetActive(false);
        mensagemPapelDireita.transform.parent.gameObject.SetActive(true);
        mensagemPapelDireita.text = FalaDoutor[faseCorrente];

        etapaAtual = "DialogoMedicoPaciente";
    }

    IEnumerator pacienteCurado()
    {
        // Reposicionar e/ou desligar elementos do cenário
        barraInfoAcoes.SetActive(false);
        FindObjectOfType<ConcedePilula>().barraInferior.gameObject.SetActive(false);
        doutorCenario.transform.localPosition = new Vector3(1f, 0f, -12.9f);
        doutorCenario.transform.localEulerAngles = new Vector3(0f, -45f, 0f);

        // Aguardar dois segundos
        yield return new WaitForSeconds(2);

        // Conta quantas pílulas ficaram no organismo (para descontar da pontuação)
        int qtdPilulasRestantesNoLab = 0;
        foreach (GameObject go in controladorLabirinto.pilulasCriadas)
            if (go != null) qtdPilulasRestantesNoLab++;

        // Concede os pontos ao jogador
        // Regra: serão concedidos 100 pontos, mais a quantidade de segundos restantes, menos 10 pontos
        // por cada pílula que permaneceu no corpo do paciente.
        // No mínimo, serão concedidos 10 pontos ao se passar de fase.
        
        Geral.PlacarCorrente += Mathf.Max(100 + Mathf.RoundToInt(tempoRestante) - (10 * qtdPilulasRestantesNoLab), 10);

        // Caso o jogo esteja em modo de dificuldade alta, 100 pontos adicionais serão concedidos, além dos já computados. 
        if (Geral.Dificuldade == "Alta")
            Geral.PlacarCorrente += 100;

        if (Geral.PlacarCorrente > Geral.RecordeAtual)
        {
            Geral.RecordeAtual = Geral.PlacarCorrente;
            if (Geral.RecordeAtual > PlayerPrefs.GetInt("RecordePontuacao"))
                PlayerPrefs.SetInt("RecordePontuacao", Geral.RecordeAtual);
        }

        // Remover paciente da cama + desligar Canvas do labirinto
        pacienteNoLeito.SetActive(false);
        visualizacaoLabirinto.SetActive(false);

        // Posicionar e rotacionar paciente novamente no centro do cenário
        pacienteCenario.transform.position = posicaoDialogoPaciente;
        pacienteCenario.transform.LookAt(doutorConversa.transform);
        pacienteCenario.SetActive(true);
        pacienteCenario.GetComponent<Animator>().SetBool("caminharPeloCenario", false);

        doutorConversa.transform.LookAt(pacienteCenario.transform);
        doutorConversa.SetActive(true);

        // Troca de câmera
        camPacienteIndoAoLeito.gameObject.SetActive(true);

        // Ativa a mensagem, exibe na tela e liga o colisor do Canvas
        mensagemPapelDireita.transform.parent.gameObject.SetActive(true);
        mensagemPapelDireita.text = FalaFinal[faseCorrente];
        objetoColliderCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(objetoColliderCanvas);

        // Executa efeito sonoro de vitória
        somVitoria.Play();

        // Reexibe a seta do mouse, caso aplicável
        if (Geral.ModoDeControle == "Teclado+Mouse")
            Cursor.lockState = CursorLockMode.None;

        etapaAtual = "DialogoFinal";
    }

    IEnumerator caminhadaPacienteLeito()
    {
        // Desligar itens do Canvas
        objetoColliderCanvas.SetActive(false);
        mensagemPapelDireita.transform.parent.gameObject.SetActive(false);

        // Iniciar caminhada
        pacienteCenario.GetComponent<Animator>().SetBool("caminharPeloCenario", true);
        etapaAtual = "PacienteCaminhadaAoLeito";

        // Interromper caminhada quando paciente chegar bem perto de posicaoPacienteIndoAoLeito
        while (Vector3.Distance(pacienteCenario.transform.position, posicaoPacienteIndoAoLeito) > 0.25f)
        {
            pacienteCenario.transform.LookAt(posicaoPacienteIndoAoLeito);
            doutorConversa.transform.LookAt(pacienteCenario.transform.position);
            yield return new WaitForEndOfFrame();
        }

        // Solicitar ao controlador do labirinto que realize as ações
        // para a devida montagem de seus elementos internos
        controladorLabirinto.LimparValoresVetoresVariaveis();
        controladorLabirinto.ImportarValoresVetoresVariaveis(faseCorrente, PosicaoMonstrosAzuis, PosicaoMonstrosVerdes,
            PosicaoMonstrosRoxos, PosicaoMonstrosAmarelos, PosicaoMonstrosVermelhos, PosicaoObstaculos, DimensaoObstaculos, PosicaoInicialPilulas,
            out qtdMonstrosAzuisRestantes, out qtdMonstrosVerdesRestantes, out qtdMonstrosRoxosRestantes, out qtdMonstrosAmarelosRestantes, out qtdMonstrosVermelhosRestantes);
        controladorLabirinto.Montar();

        AtualizarMensagemAgentesRestantes();

        // Mostrar o paciente no leito e suas doenças por 3 segundos
        MostrarLabirintoTemporariamente(3,true);
    }

    public void TransicaoParaTratamento(bool ehNovoTratamento)
    {
        // Desligar visualização e acionar doutorCenario
        foreach (Camera camTratamento in camerasTratamento)
            camTratamento.gameObject.SetActive(false);

        visualizacaoLabirinto.SetActive(false);
        doutorCenario.SetActive(true);
        etapaAtual = "Tratamento";

        if (ehNovoTratamento)
        {
            if (Geral.Dificuldade == "Alta")
                tempoRestante = TempoMaxTratamento[faseCorrente] * 0.75f;
            else
                tempoRestante = TempoMaxTratamento[faseCorrente];
        }

        coracaoLeito.SetActive(true);
        barraInfoAcoes.SetActive(true);
        Geral.ModoDeJogoCorrente = "BuscaMedicamentos";

        // Pausar os comportamentos das pílulas e dos agentes de doenças
        PausarPilulasAgentes(true);

        // Iniciar trilha sonora de ação
        if (ehNovoTratamento)
        {
            foreach (AudioSource audio in FindObjectsOfType<AudioSource>())
                audio.Stop();
            fundoSonoroAcao.Play();
        }

        // Oculta a seta do mouse, caso aplicável
        if (Geral.ModoDeControle == "Teclado+Mouse")
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void ProximaEtapa()
    {
        switch (etapaAtual) 
        {
            case "DialogoPacienteMedico":
                MostrarMensagemMedico();
                break;
            case "DialogoMedicoPaciente":
                StartCoroutine(caminhadaPacienteLeito());
                break;
            case "Tratamento":
                etapaAtual = "CarregandoDialogoFinal";
                StartCoroutine(pacienteCurado());
                break;
            case "DialogoFinal":
                AvancarProximaFase();
                break;
        }
    }

    public void MostrarLabirintoTemporariamente(int segundos, bool ehNovoTratamento)
    {
        StartCoroutine(MostrarLabirintoPacientes(segundos, ehNovoTratamento));
    }

    private IEnumerator MostrarLabirintoPacientes(int segundos, bool ehNovoTratamento, bool controlarLabirinto = false)
    {
        pacienteCenario.SetActive(false);
        
        if (ehNovoTratamento)
            doutorConversa.SetActive(false);
        else
            doutorCenario.SetActive(false);

        pacienteNoLeito.SetActive(true);
        camPacienteIndoAoLeito.gameObject.SetActive(false);

        foreach (Camera camTratamento in camerasTratamento)
            camTratamento.gameObject.SetActive(true);
        controladorLabirinto.gameObject.SetActive(true);
        visualizacaoLabirinto.SetActive(true);

        PausarPilulasAgentes(false);

        yield return new WaitForSeconds(segundos);

        if ((controlarLabirinto == false) && (etapaAtual != "CarregandoDialogoFinal"))
            TransicaoParaTratamento(ehNovoTratamento);
    }

    public void ReduzirContadorDoenca(bool doencaVerde, bool doencaVermelha, bool doencaAmarela, bool doencaAzul, bool doencaRoxa)
    {
        if (doencaVerde) qtdMonstrosVerdesRestantes--;
        if (doencaVermelha) qtdMonstrosVermelhosRestantes--;
        if (doencaAmarela) qtdMonstrosAmarelosRestantes--;
        if (doencaAzul) qtdMonstrosAzuisRestantes--;
        if (doencaRoxa) qtdMonstrosRoxosRestantes--;

        efeitoSonoroBom.Play();
    }

    public void AtualizarMensagemAgentesRestantes()
    {
        string textoComposto = "A tratar:";

        if (qtdMonstrosAzuisRestantes > 0)
            textoComposto += " Azuis=" + qtdMonstrosAzuisRestantes;
        if (qtdMonstrosVerdesRestantes > 0)
            textoComposto += " Verdes=" + qtdMonstrosVerdesRestantes;
        if (qtdMonstrosRoxosRestantes > 0)
            textoComposto += " Roxos=" + qtdMonstrosRoxosRestantes;
        if (qtdMonstrosAmarelosRestantes > 0)
            textoComposto += " Amarelos=" + qtdMonstrosAmarelosRestantes;
        if (qtdMonstrosVermelhosRestantes > 0)
            textoComposto += " Vermelhos=" + qtdMonstrosVermelhosRestantes;

        textoMensagemMomento.text = textoComposto;
    }

    public void IniciarControleLabirinto()
    {
        Geral.ModoDeJogoCorrente = "Labirinto";
        StartCoroutine(MostrarLabirintoPacientes(0, false, true));
    }

    public void InterromperControleLabirinto()
    {
        TransicaoParaTratamento(false);
    }

    public void VerificarCondicoesFinalizacaoFase()
    {
        int qtdMonstros = 0;

        qtdMonstros += qtdMonstrosVerdesRestantes;
        qtdMonstros += qtdMonstrosVermelhosRestantes;
        qtdMonstros += qtdMonstrosAmarelosRestantes;
        qtdMonstros += qtdMonstrosAzuisRestantes;
        qtdMonstros += qtdMonstrosRoxosRestantes;

        if (qtdMonstros == 0)
        {
            Geral.ModoDeJogoCorrente = "Consultorio";
            ProximaEtapa();
        }        
    }

    public void AvancarProximaFase()
    {
        Geral.ModoDeJogoCorrente = "Consultorio";
        faseCorrente++;

        // Caso não haja mais fases a cumprir, o jogador venceu o desafio;
        // Caso contrário, a próxima fase será chamada pelo game.
        if (faseCorrente == TipoPaciente.Length)
        {
            etapaAtual = "GanhouTudo";
            objetoColliderCanvas.SetActive(false);
            mensagemPapelDireita.transform.parent.gameObject.SetActive(false);
            doutorFelicidade.SetActive(true);
            telaVitoria.SetActive(true);
            telaVitoria.GetComponentInChildren<Button>().Select();
        }            
        else 
            AtenderPaciente();
    }

    public void PausarPilulasAgentes(bool pausar)
    {
        foreach (Rigidbody pilulaTratamento in FindObjectsOfType<Rigidbody>())
            if (pilulaTratamento.tag.StartsWith("medicamento"))
                pilulaTratamento.isKinematic = pausar;

        foreach (ComportamentoDoenca agenteDoenca in FindObjectsOfType<ComportamentoDoenca>(true))
            agenteDoenca.enabled = !pausar;
    }

    public void VerificacaoInicialRecorde()
    {
        if (PlayerPrefs.HasKey("RecordePontuacao"))
            Geral.RecordeAtual = PlayerPrefs.GetInt("RecordePontuacao");
        else PlayerPrefs.SetInt("RecordePontuacao", 0);

        // Ajuste dos corações do menu inicial
        // Será exibido um coração a cada 150 pontos, até um limite máximo de 12 corações

        int auxiliarQtdCoracoes = Geral.RecordeAtual;
        for (int i = 0; i < 12; i++)
            if (auxiliarQtdCoracoes > 0)
            {
                GameObject newCoracao = Instantiate(coracaoRecordeRef.gameObject);
                newCoracao.transform.SetParent(coracaoRecordeRef.transform.parent);
                newCoracao.transform.localScale = Vector3.one;
                newCoracao.SetActive(true);

                auxiliarQtdCoracoes -= 150;
            }
    }

    public void RecarregarCenaAtual()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

