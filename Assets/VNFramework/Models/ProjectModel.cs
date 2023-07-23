using UnityEngine;
namespace VNFramework
{
    class ProjectModel : AbstractModel
    {
        private string _title;
        private string _titleBgp;
        private string _titleBgm;

        public string TitlePic { get => _title; set => _title = value; }
        public string TitleBgp { get => _titleBgp; set => _titleBgp = value; }
        public string TitleBgm { get => _titleBgm; set => _titleBgm = value; }


        public void PrintProjectModel()
        {
            Debug.Log(@$"Title : {_title}
StartUpViewBgp : {_titleBgp}
StartUpViewBgm : {_titleBgm}");
        }

        protected override void OnInit()
        {
            this.GetUtility<GameDataStorage>().LoadProjectData();
        }
    }
}