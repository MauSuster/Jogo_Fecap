using UnityEngine;

public class DegrauSorteador : MonoBehaviour
{
    public StepByStepClimber capsula; // Refer�ncia � c�psula que ir� subir os degraus
    public int maxDegraus = 15;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Gera um n�mero aleat�rio de degraus que a c�psula tentar� subir
            int sorteado = Random.Range(1, maxDegraus + 1);

            // Envia para o Console da Unity o n�mero de degraus sorteado
            Debug.Log("N�mero sorteado de degraus: " + sorteado);

            // Chama a fun��o no script da c�psula para que ela tente subir o n�mero sorteado de degraus
            capsula.SubirDegraus(sorteado);
        }
    }
}
