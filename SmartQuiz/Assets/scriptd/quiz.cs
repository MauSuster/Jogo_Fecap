using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class QuizController : MonoBehaviour
{
    [Header("Referências")]
    public StepByStepClimber[] capsulas;
    public TextMeshProUGUI textoPergunta;
    public Button botaoA, botaoB, botaoC, botaoD;
    public TurnBasedSorter turnBasedSorter;

    [Header("Configuração da API")]
    [SerializeField] private string chave_api = "endrew123";
    [SerializeField] private int quantidadeQuestoes = 5;
    [SerializeField] private string url = "https://script.google.com/macros/s/AKfycbxWwdShZwyJRdEF55T2FEE2wl5xbyfYbgsBbW9kjE75DZDi6_JTegYqYvSq4-rXa70C/exec";

    private Questao[] questoes;
    private int questaoAtual = 0;

    void Start()
    {
#if UNITY_EDITOR
        Debug.ClearDeveloperConsole();
#endif
        botaoA.onClick.AddListener(() => StartCoroutine(Responder(1)));
        botaoB.onClick.AddListener(() => StartCoroutine(Responder(2)));
        botaoC.onClick.AddListener(() => StartCoroutine(Responder(3)));
        botaoD.onClick.AddListener(() => StartCoroutine(Responder(4)));

        StartCoroutine(BuscarQuestoes());
    }

    IEnumerator BuscarQuestoes()
    {
        var postData = new Dictionary<string, object>
        {
            { "chave_api", chave_api },
            { "quantidade", quantidadeQuestoes }
        };

        string json = JsonUtility.ToJson(new SerializableDict(postData));
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
{
    Debug.LogError("Erro na requisição: " + request.error);
}
else
{
    var wrapper = JsonUtility.FromJson<QuestoesWrapper>(request.downloadHandler.text);
    if (wrapper.questoes != null && wrapper.questoes.Length > 0)
    {
        questoes = wrapper.questoes;
        ExibirQuestao();
    }
    else
    {
        Debug.LogError("Nenhuma questão foi recebida.");
    }
}
    }

    void ExibirQuestao()
    {
        if (questaoAtual >= questoes.Length)
        {
            textoPergunta.text = "Fim do quiz!";
            SetBotoesAtivos(false);
            return;
        }

        var q = questoes[questaoAtual];
        textoPergunta.text = q.Pergunta;
        botaoA.GetComponentInChildren<TextMeshProUGUI>().text = "A: " + q.A;
        botaoB.GetComponentInChildren<TextMeshProUGUI>().text = "B: " + q.B;
        botaoC.GetComponentInChildren<TextMeshProUGUI>().text = "C: " + q.C;
        botaoD.GetComponentInChildren<TextMeshProUGUI>().text = "D: " + q.D;
    }

    IEnumerator Responder(int respostaEscolhida)
    {
        SetBotoesAtivos(false);
        var q = questoes[questaoAtual];

        Button botaoCorreto = ObterBotaoPorIndice(q.Resposta);
        Button botaoSelecionado = ObterBotaoPorIndice(respostaEscolhida);

        Color corCorreta = new Color(0.3f, 0.8f, 0.3f);
        Color corErrada = new Color(0.9f, 0.3f, 0.3f);

        botaoCorreto.image.color = corCorreta;

        if (botaoSelecionado != botaoCorreto)
        {
            botaoSelecionado.image.color = corErrada;
        }
        else
        {
            turnBasedSorter.SubirDegrau();
        }

        yield return new WaitForSeconds(4f);

        ResetarCoresDosBotoes();
        turnBasedSorter.TrocaVez();
        questaoAtual++;
        ExibirQuestao();
        SetBotoesAtivos(true);
    }

    void ResetarCoresDosBotoes()
    {
        Color corPadrao = Color.white;
        botaoA.image.color = corPadrao;
        botaoB.image.color = corPadrao;
        botaoC.image.color = corPadrao;
        botaoD.image.color = corPadrao;
    }

    void SetBotoesAtivos(bool ativo)
    {
        botaoA.interactable = ativo;
        botaoB.interactable = ativo;
        botaoC.interactable = ativo;
        botaoD.interactable = ativo;
    }

    Button ObterBotaoPorIndice(int indice)
    {
        return indice switch
        {
            1 => botaoA,
            2 => botaoB,
            3 => botaoC,
            4 => botaoD,
            _ => null
        };
    }

    [System.Serializable]
    public class Questao
    {
        public int ID;
        public string Pergunta;
        public string A, B, C, D;
        public int Resposta;
    }

    [System.Serializable]
    public class QuestoesWrapper
    {
        public Questao[] questoes;
    }

    [System.Serializable]
    public class SerializableDict
    {
        public string chave_api;
        public int quantidade;

        public SerializableDict(Dictionary<string, object> dict)
        {
            chave_api = dict["chave_api"].ToString();
            quantidade = int.Parse(dict["quantidade"].ToString());
        }
    }
}
