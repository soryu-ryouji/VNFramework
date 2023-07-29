using System.Collections.Generic;
using System.Linq;

namespace VNFramework
{
    public class ChapterInfo
    {
        public string ChapterName;
        public string FileName;
        public string Resume;
        public string ResumePic;
    }

    class ChapterModel : AbstractModel
    {
        private List<string> _unlockedChapterList;
        private ChapterInfo[] _chapterInfoList;

        private string mCurrentChapter;

        public List<string> UnlockedChapterList { get => _unlockedChapterList; set => _unlockedChapterList = value; }
        public ChapterInfo[] ChapterInfoList { get => _chapterInfoList; set => _chapterInfoList = value; }
        public string CurrentChapter { get => mCurrentChapter; set => mCurrentChapter = value; }

        public string GetFileName(string chapterName)
        {
            ChapterInfo chapterInfo = ChapterInfoList.FirstOrDefault(info => info.ChapterName == chapterName);

            return chapterInfo?.FileName ?? "";
        }

        public ChapterInfo GetChapterInfo(string chapterName)
        {
            ChapterInfo chapterInfo = ChapterInfoList.FirstOrDefault(info => info.ChapterName == chapterName);

            return chapterInfo;
        }

        public void AddUnlockedChapter(string chapterName)
        {
            _unlockedChapterList.Add(chapterName);
            // 更新本地记录
            this.GetUtility<GameDataStorage>().SaveUnlockedChapterList();
        }
        protected override void OnInit()
        {
            _unlockedChapterList = new(this.GetUtility<GameDataStorage>().LoadUnlockedChapterList());
            _chapterInfoList = this.GetUtility<GameDataStorage>().LoadChapterInfoList();
        }
    }
}