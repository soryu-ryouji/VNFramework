namespace VNFramework
{
    class UnlockedChapterCommand : AbstractCommand
    {
        string _currentChapterName;
        public UnlockedChapterCommand(string currentChapterName)
        {
            _currentChapterName = currentChapterName;
        }

        protected override void OnExecute()
        {
            var model = this.GetModel<ChapterModel>();
            var unlockedList = model.UnlockedChapterList;
            var chapterList = model.ChapterInfoList;

            // 若通关的章节并不是最新的已解锁章节，则什么也不做
            if (unlockedList[unlockedList.Count -1] != _currentChapterName) return;
            for (int i = 0; i < chapterList.Length; i++)
            {
                if (chapterList[i].ChapterName == _currentChapterName && i + 1 < chapterList.Length)
                {
                    model.AddUnlockedChapter(chapterList[i + 1].ChapterName);
                }
            }
        }
    }
}