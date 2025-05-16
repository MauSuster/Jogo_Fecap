using UnityEngine;

public class DegrauSorteador : MonoBehaviour
{
    public StepByStepClimber capsula; // Referência à cápsula que irá subir os degraus
    public int maxDegraus = 15;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Gera um número aleatório de degraus que a cápsula tentará subir
            int sorteado = Random.Range(1, maxDegraus + 1);

            // Envia para o Console da Unity o número de degraus sorteado
            Debug.Log("Número sorteado de degraus: " + sorteado);

            // Chama a função no script da cápsula para que ela tente subir o número sorteado de degraus
            capsula.SubirDegraus(sorteado);
        }
    }
}
