using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Não se esqueça de importar TextMeshPro
using UnityEngine.SceneManagement; // Importando o SceneManager para gerenciar as cenas

public class StepByStepClimber : MonoBehaviour
{
    public Transform stairParent; // O objeto pai que agrupa todos os degraus da escada
    public float stepSpeed = 0.5f;
    public float pauseBetweenSteps = 0.2f;
    public float offsetLateral = 0f; // Deslocamento lateral para evitar colisão entre cápsulas
    private List<Transform> steps = new List<Transform>();  // Lista para armazenar os Transform de cada degrau
    private bool isClimbing = false; // Flag para indicar se a cápsula está atualmente subindo
    private int degrauAtual = 0; // Índice do degrau atual que a cápsula está prestes a subir ou já subiu
    public CameraFollower cameraFollower;

    public int pontosSubidos = 0; // Adiciona a pontuação para escadas subidas
    public TextMeshProUGUI tmpText; // Referência ao TextMeshProUGUI para exibir a pontuação
    public TextMeshProUGUI vitoriaTexto; // Referência ao TextMeshProUGUI para exibir a mensagem de vitória

    void Start()
    {
        // Preenche a lista de degraus com os filhos do stairParent
        foreach (Transform step in stairParent)
        {
            steps.Add(step);
        }

        // Ordena os degraus por altura (Y)
        steps.Sort((a, b) => a.position.y.CompareTo(b.position.y));
        Debug.Log($"{gameObject.name}: Degraus encontrados: " + steps.Count);

        // Atualiza o texto com a pontuação inicial
        if (tmpText != null)
        {
            tmpText.text = "Pontuação: 0";
        }

        // Inicializa o texto de vitória como vazio
        if (vitoriaTexto != null)
        {
            vitoriaTexto.text = "";
        }
    }

    void Update() { }

    // Responsável por mover a cápsula de degrau em degrau
    public IEnumerator ClimbSteps(int quantidade)
    {
        isClimbing = true;

        // Calcula quantos degraus ainda restam na escada
        int degrausRestantes = steps.Count - degrauAtual;

        // Garante que a quantidade a subir não seja negativa ou maior que os degraus restantes
        int degrausASubir = Mathf.Clamp(quantidade, 0, degrausRestantes);

        // Loop para subir a quantidade de degraus especificada
        for (int i = 0; i < degrausASubir; i++)
        {
            // Obtém o Transform do degrau atual para o qual a cápsula deve se mover
            Transform step = steps[degrauAtual];
            Vector3 destinoComOffset = step.position + new Vector3(offsetLateral, 0f, 0f);

            while (Vector3.Distance(transform.position, destinoComOffset) > 0f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destinoComOffset, stepSpeed * Time.deltaTime);
                yield return null;
            }

            // Espera um pequeno intervalo de tempo entre a subida de cada degrau
            yield return new WaitForSeconds(pauseBetweenSteps);

            // Incrementa o índice do degrau atual e a pontuação
            degrauAtual++;
            pontosSubidos++; // Incrementa a pontuação

            // Verifica se a cápsula chegou ao quinto degrau
            if (degrauAtual == 3)
            {
                // A cápsula chegou no quinto degrau, então o jogo deve terminar
                FimDeJogo();
                yield break; // Interrompe a corrotina, finalizando o movimento
            }
        }

        isClimbing = false;

        // Atualiza o texto da pontuação na interface
        if (tmpText != null)
        {
            tmpText.text = "Pontuação: " + pontosSubidos;
        }
    }

    // Chamado externamente (pelo sorteador)
    public void SubirDegraus(int quantidade)
    {
        if (cameraFollower != null)
        {
            cameraFollower.SetTarget(this.transform); // Atualiza o alvo da câmera para a cápsula atual
        }

        // Inicia uma corrotina que espera 1 segundo antes de começar a subir
        StartCoroutine(EsperarESubirDegraus(quantidade));
    }

    public void TrocaCamera(int capsula, StepByStepClimber[] capsulas)
    {
        if (cameraFollower != null && capsulas != null && capsula >= 0 && capsula < capsulas.Length)
        {
            cameraFollower.SetTarget(capsulas[capsula].transform); // Troca o alvo da câmera para a cápsula correta
        }
    }

    private IEnumerator EsperarESubirDegraus(int quantidade)
    {
        // Espera 1 segundo
        yield return new WaitForSeconds(1f);

        if (!isClimbing && degrauAtual < steps.Count)
        {
            StartCoroutine(ClimbSteps(quantidade)); // Inicia a subida
        }
    }

    // Função que é chamada quando o jogo termina
    private void FimDeJogo()
    {
        // Aqui você pode fazer algo como desabilitar a cápsula, parar a contagem de pontos, etc.
        Debug.Log("Fim de Jogo! Você Ganhou!");

        // Exibe a mensagem de vitória
        if (vitoriaTexto != null)
        {
            vitoriaTexto.text = "Você Ganhou!";
        }

        // Carrega a cena do menu após 2 segundos
        StartCoroutine(VoltarParaMenu());
    }

    private IEnumerator VoltarParaMenu()
    {
        // Espera 2 segundos para mostrar a mensagem de vitória
        yield return new WaitForSeconds(2f);

        // Carrega a cena do menu
        SceneManager.LoadScene("Menu"); // Substitua "Menu" pelo nome da sua cena de menu
    }

    // Opcional: método pra resetar a cápsula se quiser reiniciar a corrida
    public void Resetar()
    {
        if (steps.Count > 0)
        {
            transform.position = steps[0].position + new Vector3(offsetLateral, 0f, 0f);
            degrauAtual = 0;
            pontosSubidos = 0; // Reseta a pontuação
            isClimbing = false;

            // Reseta o texto da pontuação na interface
            if (tmpText != null)
            {
                tmpText.text = "Pontuação: 0";
            }

            // Reseta a mensagem de vitória
            if (vitoriaTexto != null)
            {
                vitoriaTexto.text = "";
            }
        }
    }
}
