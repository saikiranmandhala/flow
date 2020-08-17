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
    public class Transition
    {
        public string CurrentStatus { get; set; }
        public string CurrentPhase { get; set; }
        public string Action { get; set; }
        public string Condition { get; set; }
        public string ResultStatus { get; set; }
        public string ResultPhase { get; set; }
        public List<Role> Roles { get; set; }
        public Object PostTransition { get; set; }
    }
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
    }
}
