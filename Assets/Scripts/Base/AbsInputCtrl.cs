using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCtrl
{ 
    public class AbsInputCtrl : MonoBehaviour
    {
        protected delegate void ParamCollector(in ActionParam param);

        protected List<KeyValuePair<AbsAction, ParamCollector>> action2InputJudge = new List<KeyValuePair<AbsAction, ParamCollector>>();



        void Awake()
        {
            InitialAction2InputJudge();
        }

        protected virtual void InitialAction2InputJudge()
        {
 
        }

        protected void appendAction2InputJudge(AbsAction action, ParamCollector judge)
        {
            KeyValuePair<AbsAction, ParamCollector> pair = new KeyValuePair<AbsAction, ParamCollector>(action, judge);
            action2InputJudge.Add(pair);
            
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            executeActions();
        }

        void FixedUpdate()
        {
            triggerActions();
        }

        private void triggerActions()
        {
            action2InputJudge.ForEach(delegate (KeyValuePair<AbsAction, ParamCollector> pair)
            {
                ParamCollector inputJudge = pair.Value;
                AbsAction action = pair.Key;
                ActionParam param =  action.GetActionParam();
                inputJudge(param);
                action.trigger(param);
               
            });
        }

        protected void executeActions()
        {
            action2InputJudge.ForEach(delegate (KeyValuePair<AbsAction, ParamCollector> pair)
            {
          
                AbsAction action = pair.Key;
                action.execute();

            });
        }
    }

}
