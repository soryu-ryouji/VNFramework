using System.Collections.Generic;

namespace VNFramework
{
    class SaveFileModel : AbstractModel
    {
        private SaveFile[] _saveFiles;

        public SaveFile[] GetSaveFiles()
        {
            return _saveFiles;
        }

        protected override void OnInit()
        {
            _saveFiles = this.GetUtility<GameDataStorage>().LoadSaveFiles();
        }
    }
}