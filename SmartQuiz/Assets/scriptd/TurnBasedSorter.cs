using UnityEngine;

public class TurnBasedSorter : MonoBehaviour
{
    public StepByStepClimber[] capsulas;
    private int vezAtual = 0;

    public void Update()
    {
        // Tecla J → passa a vez sem andar e troca a câmera
        if (Input.GetKeyDown(KeyCode.J))
        {
            StepByStepClimber atual = capsulas[vezAtual];
            Debug.Log($"Cápsula {vezAtual + 1} passou a vez");

            // Troca a vez
            vezAtual = (vezAtual + 1) % capsulas.Length;

            // Troca a câmera para a próxima cápsula
            atual.TrocaCamera(vezAtual, capsulas);
        }

        // Tecla P → exibe a pontuação das cápsulas
        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < capsulas.Length; i++)
            {
                Debug.Log($"Cápsula {i + 1} subiu {capsulas[i].pontosSubidos} degraus");
            }
        }
    }

    // Método para subir a cápsula automaticamente (quando a resposta for correta)
    public void SubirDegrau()
    {
        StepByStepClimber atual = capsulas[vezAtual];

        if (atual != null)
        {
            Debug.Log($"Cápsula {vezAtual + 1} sobe 1 degrau");
            atual.SubirDegraus(1);  // A cápsula da vez sobe 1 degrau
        }
    }
    public void TrocaVez()
{
    vezAtual = (vezAtual + 1) % capsulas.Length; // Avança para o próximo jogador

    StepByStepClimber atual = capsulas[vezAtual];
    if (atual != null)
    {
        atual.TrocaCamera(vezAtual, capsulas);
    }
}

}
