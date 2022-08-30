using System.Collections.Generic;
using LoadScenes;
using MaoUtility.Converse.Core;
using Menu;
using ServiceScript;

namespace MaoUtility.Converse.ConectorFiles
{
    public class RegisterConverse : RegisterServicesScene
    {
        public List<Data> Datas;
        
        [System.Serializable]
        public class Data
        {
            public ConversePanelCreator Creator;
            public string Name;
        }

        public override void Register()
        {
            foreach (var data in Datas)
            {
                if (string.IsNullOrWhiteSpace(data.Name) == false)
                    ServicesID<ConversePanelCreator>.S.Set(data.Creator, data.Name);
                else
                    Services<ConversePanelCreator>.S.Set(data.Creator);
            }
        }

        public override void Unregister()
        {
            foreach (var data in Datas)
            {
                if(string.IsNullOrWhiteSpace(data.Name)==false)
                    ServicesID<ConversePanelCreator>.S.Set(null, data.Name);
                else
                    Services<ConversePanelCreator>.S.Set(null); 
            }
        }
    }
}