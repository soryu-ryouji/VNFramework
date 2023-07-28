using System.Collections;
using UnityEngine;

namespace VNFramework
{
    public static class VNutils
    {
        public static Hashtable Hash(params object[] args)
        {
            Hashtable hashTable = new Hashtable(args.Length / 2);
            if (args.Length % 2 != 0)
            {
                Debug.LogError("VN Framework Error: Hash requires an even number of arguments!");
                return null;
            }
            else
            {
                int i = 0;
                while (i < args.Length - 1)
                {
                    hashTable.Add(args[i], args[i + 1]);
                    i += 2;
                }
                return hashTable;
            }
        }

        public static SpriteObj StrToSpriteObj(string obj)
        {
            if (obj == "bgp") return SpriteObj.Bgp;
            else if (obj == "ch_left") return SpriteObj.ChLeft;
            else if (obj == "ch_right") return SpriteObj.ChRight;
            else if (obj == "ch_mid") return SpriteObj.ChMid;
            else return SpriteObj.Null;
        }

        public static AudioPlayer StrToAudioPlayer(string obj)
        {
            if (obj == "bgm") return AudioPlayer.Bgm;
            else if (obj == "bgs") return AudioPlayer.Bgs;
            else if (obj == "chs") return AudioPlayer.Chs;
            else if (obj == "gms") return AudioPlayer.Gms;
            else return AudioPlayer.Null;
        }
    }
}