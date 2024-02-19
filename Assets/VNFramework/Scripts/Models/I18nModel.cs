using System.Collections.Generic;
using Newtonsoft.Json;

namespace VNFramework
{
    using TranslationBundle = Dictionary<string, object>;
    class I18nModel : AbstractModel, ICanRegisterEvent, ICanGetModel
    {
        private readonly Dictionary<string, TranslationBundle> TranslationBundles = new();
        public readonly string[] SupportedLocales ={"Chinese", "English"};
        public string DefaultLocale => SupportedLocales[0];
        public string CurrentLocale;

        public string __(string ResKey)
        {
            return TranslationBundles[CurrentLocale][ResKey] as string;
        }

        public void InitModel()
        {
            CurrentLocale = this.GetModel<ConfigModel>().Language;
            this.RegisterEvent<LanguageChangedEvent>(_ => CurrentLocale = this.GetModel<ConfigModel>().Language);
            foreach(var lang in SupportedLocales)
            {
                var i18nText = this.GetUtility<GameDataStorage>().LoadI18nRes(lang);
               TranslationBundles[lang] = JsonConvert.DeserializeObject<TranslationBundle>(i18nText);
            }
        }

        protected override void OnInit()
        {

        }
    }
}