using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCtrl
{ 
    public class AbsInputCtrl : MonoBehaviour
    {
        protected delegate bool InputJudge();

        protected List<KeyValuePair<AbsAction, InputJudge>> action2InputJudge;


        void Awake()
        {
            initialAction2InputJudge();
        }

        protected virtual void initialAction2InputJudge()
        {

        }

        protected void appendAction2InputJudge(AbsAction action, InputJudge judge)
        {
            KeyValuePair<AbsAction, InputJudge> pair = new KeyValuePair<AbsAction, InputJudge>(action, judge);
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
            action2InputJudge.ForEach(delegate (KeyValuePair<AbsAction, InputJudge> pair)
            {
                InputJudge inputJudge = pair.Value;
                AbsAction action = pair.Key;
                if (inputJudge())
                {
                    action.trigger(true);
                }
                else
                {
                    action.trigger(false);
                }

            });
        }

        private void executeActions()
        {
            action2InputJudge.ForEach(delegate (KeyValuePair<AbsAction, InputJudge> pair)
            {
          
                AbsAction action = pair.Key;
                action.execute();

            });
        }
    }

}
