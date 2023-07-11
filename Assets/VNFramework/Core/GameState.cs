using System;
using System.Collections;
using UnityEngine.Events;

namespace VNFramework
{
    public static class GameState
    {
        public static Func<bool> IsDialogueTyping;
        public static UnityAction DialogueStop;
        public static UnityAction<Hashtable> DialogueChanged;
        public static UnityAction<Hashtable> NameChanged;

        public static UnityAction<Hashtable> AudioChanged;

        public static UnityAction<Hashtable> BgpChanged;
        public static UnityAction<Hashtable> ChlpChanged;
        public static UnityAction<Hashtable> ChmpChanged;
        public static UnityAction<Hashtable> ChrpChanged;

        public static UnityAction<Hashtable> UIChanged;

        public static UnityAction NextCommand;
    }
}