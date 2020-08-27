using System.Collections.Generic;

namespace WorkflowModule
{
    public class Transition
    {
        public int Id { get; set; }
        public string CurrentStatus { get; set; }
        public string CurrentPhase { get; set; }
        public string Action { get; set; }
        public string NextStatus { get; set; }
        public string NextPhase { get; set; }

        public List<Role> Roles { get; set; }

        public int SLAId { get; set; }
        public string Precondition { get; set; }
        public string PostTransition { get; set; }

        public int? ChildTransitionId { get; set; }
        public int? ParentTransitionId { get; set; }

    }

    //public class CaseTransition : Transition
    //{
    //    public TaskTransition Task { get; set; }

    //}
    public class TaskTransition : Transition
    {
        public TaskType TaskType { get; set; }

    }

    public enum TaskType
    {
        Parallel, Review
    }

    public class SLA
    {
        public int SLAId { get; set; }
        public string SLACode { get; set; }
        public string SLAName { get; set; }
    }
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
    }
}