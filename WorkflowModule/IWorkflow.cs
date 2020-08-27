using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowModule
{

    public interface IWorkflow
    {

        List<Transition> Transitions { get; }

        Transition GetTransition(Transition transition);

        Transition ExecuteTransition(Transition transition);

    }
    
}
