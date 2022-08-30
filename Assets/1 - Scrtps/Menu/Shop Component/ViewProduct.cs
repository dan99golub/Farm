using System;
using DTO;
using ServiceScript;
using TMPro;
using UltEvents;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Shop_Component
{
    public class ViewProduct<T> : MonoBehaviour where T : MonoBehaviour
    {
        public TextMeshProUGUI CostLabel;
        public Image Icon;
        public TextMeshProUGUI NameLabel;
        

        [Header("State")]
        public UltEvent IsBuy;
        public UltEvent IsNOtBuy;
        
        [Header("Can be buy")]
        public UltEvent CanBeBuyEvent;
        public UltEvent CantBeBuyEvent;

        [Header("is select")]
        public UltEvent Selected;
        public UltEvent NotSelected;
        
        
        private bool _isBuyOnStart;
        private bool _isSelectedOnStart;
        private bool _canBeBuyOnStart;
        protected Product<T> Content;

        private Progress Progress => Services<DB>.S.Get().Progress;

        public void Init(Product<T> content)
        {
            Content = content;
            Icon.sprite = content.Icon;
            CostLabel.text = content.Cost.ToString();
            NameLabel.text = content.NameContent;
            if(Progress.GetSaveProduct<T>().Has(content)==false && content.Cost<=0) BuySilent();
            
           
            if(_isBuyOnStart = Progress.GetSaveProduct<T>().Has(content))  IsBuy.Invoke(); else IsNOtBuy.Invoke();
            if(_isSelectedOnStart = Progress.GetSelectedGuidProduct<T>() == content.GUID) Selected.Invoke(); else NotSelected.Invoke();
            if(_canBeBuyOnStart = CanBeBuy()) CanBeBuyEvent.Invoke(); else CantBeBuyEvent.Invoke();
            
            Progress.Signals.Register<Updated>(OnUpdateDB);
        }

        private void OnUpdateDB(Updated e)
        {
            ActionUpdateView(()=>_isBuyOnStart, ()=>Progress.GetSaveProduct<T>().Has(Content) ,x=>_isBuyOnStart=x, value 
                => { if(value) IsBuy.Invoke();else IsNOtBuy.Invoke(); });    
            ActionUpdateView(()=>_isSelectedOnStart, ()=>Progress.GetSelectedGuidProduct<T>()==Content.GUID, x=>_isSelectedOnStart=x, v 
                => { if(v) Selected.Invoke(); else NotSelected.Invoke(); });
            ActionUpdateView(()=>_canBeBuyOnStart, ()=>CanBeBuy(), x=>_canBeBuyOnStart=x, v 
                => {if(v) CanBeBuyEvent.Invoke(); else CantBeBuyEvent.Invoke();});
        }

        private void OnDestroy()
        {
            Progress?.Signals.UnRegister<Updated>(OnUpdateDB);
        }

        private void ActionUpdateView(Func<bool> getOld, Func<bool> getNew, Action<bool> setter, Action<bool> action)
        {
            var n = getNew();
            if (getOld() != n)
            {
                setter(n);
                action(n);
            }
        }

        public void Select()
        {
            if (Progress.GetSaveProduct<T>().Has(Content))
            {
                Progress.SetSelectedGuidProduct(Content);
                Progress.Signals.Fire(new Updated());
            }
        }

        private void BuySilent()
        {
            if (CanBeBuy())
            {
                Progress.SpentMoney(Content.Cost);
                Progress.GetSaveProduct<T>().Add(Content);
            }
        }
        
        public void Buy()
        {
            if (CanBeBuy())
            {
                Progress.SpentMoney(Content.Cost);
                Progress.GetSaveProduct<T>().Add(Content);
                Progress.Signals.Fire(new Updated());
            }
        }

        public bool CanBeBuy() => Progress.CanSpentMoney(Content.Cost) && Progress.GetSaveProduct<T>().Has(Content)==false;
    }
}