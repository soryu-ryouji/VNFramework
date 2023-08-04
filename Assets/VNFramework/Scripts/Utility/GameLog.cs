using UnityEngine;
namespace VNFramework
{
    enum LogType
    {
        AsmRunning,
        Debug,
    }
    class GameLog : IUtility
    {
        public void RunningLog(string log)
        {
            Debug.Log("Running Log : " + log);
        }

        public void DebugLog(string log)
        {
            Debug.Log("Debug Log : " + log);
        }

        public void ErrorLog(string log)
        {
            Debug.LogError("Error Log : " + log);
        }
    }
}