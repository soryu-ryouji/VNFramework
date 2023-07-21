using System.Collections;
using UnityEngine.Events;

namespace VNFramework
{
    public class GameState
    {
        public static UnityAction<Hashtable> AudioChanged;
        public static UnityAction<Hashtable> BgpChanged;
        public static UnityAction<Hashtable> ChlpChanged;
        public static UnityAction<Hashtable> ChmpChanged;
        public static UnityAction<Hashtable> ChrpChanged;
    }
}