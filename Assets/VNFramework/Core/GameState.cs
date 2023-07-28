using System.Collections;
using UnityEngine.Events;

namespace VNFramework
{
    public static class GameState
    {
        public static UnityAction<Hashtable> BgmChanged;
        public static UnityAction<Hashtable> BgsChanged;
        public static UnityAction<Hashtable> ChsChanged;
        public static UnityAction<Hashtable> GmsChanged;

        public static UnityAction<Hashtable> BgpChanged;
        public static UnityAction<Hashtable> ChlpChanged;
        public static UnityAction<Hashtable> ChmpChanged;
        public static UnityAction<Hashtable> ChrpChanged;
    }
}