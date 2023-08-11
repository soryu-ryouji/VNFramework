using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VNFramework.Core;

namespace VNFramework
{
    class ChapterModel : AbstractModel
    {
        private List<ChapterInfo> _chapterInfoList;
        private List<string> _unlockedChapterList;

        public string CurrentChapter;

        public List<ChapterInfo> ChapterInfoList
        {
            get { return _chapterInfoList; }
        }

        public List<string> UnlockedChapterList
        {
            get { return _unlockedChapterList; }
        }

        public ChapterInfo GetChapterInfo(string mermaidName)
        {
            foreach (var chapterInfo in ChapterInfoList)
            {
                if (chapterInfo.MermaidName == mermaidName)
                {
                    return chapterInfo;
                }
            }

            return null;
        }

        public void TryAddUnlockedChapter(string mermaidName)
        {
            
            Debug.Log(string.Format("<color=green>{0}</color>", $"Try Add Unlocked Chapter : {mermaidName}"));
            if (_unlockedChapterList.Contains(mermaidName)) return;

            foreach (var chapter in ChapterInfoList)
            {
                if (chapter.MermaidName ==mermaidName)
                {
                    Debug.Log(string.Format("<color=green>{0}</color>", $"Add Unlocked Chapter : {mermaidName} Success"));
                    _unlockedChapterList.Add(mermaidName);
                    this.GetUtility<GameDataStorage>().SaveUnlockedChapterList();
                }
            }
        }

        public string PrintChapterInfoList()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Chapter Info List");
            foreach (var chapter in ChapterInfoList)
            {
                sb.AppendLine(chapter.MermaidName);
            }

            return sb.ToString();
        }

        protected override void OnInit()
        {
            _chapterInfoList = this.GetUtility<GameDataStorage>().LoadChapterInfoList();
            _unlockedChapterList = this.GetUtility<GameDataStorage>().LoadUnlockedChapterList();

            Debug.Log(string.Format("<color=green>{0}</color>", PrintChapterInfoList()));
        }
    }
}