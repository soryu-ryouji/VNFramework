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
        public bool isPrintAsmRunning = true;
        public bool isPrintDebug = true;
        public void Log(string log, LogType type = LogType.Debug)
        {
            if (type == LogType.AsmRunning && isPrintAsmRunning)
            {
                Debug.Log(log);
            }

            else if (type == LogType.Debug && isPrintDebug)
            {
                Debug.Log(log);
            }
        }
    }
}