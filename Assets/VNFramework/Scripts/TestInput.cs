using UnityEngine;
using VNFramework;

public class TestInput : MonoBehaviour
{

    public PerformanceController performanceController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameState.NextCommand();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameState.UIChanged(VNutils.Hash(
                "object", "dialogue",
                "action", "toggle"
            ));
        }
    }
}
