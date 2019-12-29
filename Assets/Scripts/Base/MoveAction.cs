using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCtrl
{
    public class MoveAction :AbsAction
    {
        public MoveAction()
        {
            SetIsBreakableByInput(true);

        }

        protected override void OnActionExecute() 
        {
            base.OnActionExecute();
            transform.Translate(0, 0, 1, Space.World);

        }
    }
}


