using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkflowModule
{
    public class Workflow
    {

        public Workflow LoadTransitions()
        {
            Transitions = new List<Transition>();
            Transition newAcpCase = new Transition()
            {
                Id = 11,
                CurrentPhase = string.Empty,
                CurrentStatus = string.Empty,
                Action = "New",
                NextPhase = "Intake",
                NextStatus = "PendingAssignment",
                Precondition = "WorkflowModule.ACP"
            };
            Transitions.Add(newAcpCase);

            Transition newCase = new Transition()
            {
                Id = 12,
                CurrentPhase = string.Empty,
                CurrentStatus = string.Empty,
                Action = "New",
                NextPhase = "Intake",
                NextStatus = "Unassigned",
                Precondition = string.Empty
            };
            Transitions.Add(newCase);

            Transition intakeComplete = new Transition()
            {
                Id = 13,
                CurrentPhase = "Intake",
                CurrentStatus = "PendingAssignment",
                Action = "Assign",
                NextPhase = "Assessment",
                NextStatus = "Assessment InProgress",
                Precondition = string.Empty
            };
            Transitions.Add(intakeComplete);


            Transition assessmentComplete = new Transition()
            {
                Id = 21,
                CurrentPhase = "Assessment",
                CurrentStatus = "Assessment InProgress",
                Action = "AssessmentComplete",
                NextPhase = string.Empty,
                NextStatus = string.Empty,

                Precondition = "WorkflowModule.Probation",
                ChildTransitionId = 211
            };
            Transitions.Add(assessmentComplete);


            TaskTransition managerReview = new TaskTransition()
            {
                Id = 211,
                TaskType = TaskType.Review,
                NextStatus = "Open",
                ParentTransitionId = 21
            };
            Transitions.Add(managerReview);

            TaskTransition managerReviewComplete = new TaskTransition()
            {
                Id = 212,
                
                Action = "ReviewComplete",
                NextStatus = "ReviewCompleted",
                ParentTransitionId = 21
            };
            Transitions.Add(assessmentComplete);



            Transition claimNonAcpCase = new Transition()
            {
                CurrentPhase = "Intake",
                CurrentStatus = "Assigned",
                Action = "Claim",
                NextPhase = "RootCause",
                NextStatus = "Assigned",
                Precondition = string.Empty
            };
            Transitions.Add(claimNonAcpCase);

            Transition claimAcpCase = new Transition()
            {
                CurrentPhase = "Intake",
                CurrentStatus = "Pending Assignment",
                Action = "Claim",
                NextPhase = "RootCause",
                NextStatus = "Assigned",
                Precondition = string.Empty
            };

            Transitions.Add(claimAcpCase);

            return this;
        }

        public List<Transition> Transitions { get; set; }

        public void UpdateCaseDetails(Transition transition)
        {

        }

        public Transition GetCaseDetails(string recordNumber)
        {
            Transition transition = new Transition();
            return transition;
        }

        public Transition ExecuteTransition(Transition transition)
        {
            return transition;
        }

        public Transition GetPossibleTransition(Transition transition)
        {
            List<Transition> transitions = null;
            transitions = Transitions.FindAll(tran => tran.CurrentStatus == transition.CurrentStatus
            && tran.CurrentPhase == transition.CurrentPhase
            && (!string.IsNullOrEmpty(tran.Precondition.Trim()) && RunPrecondition(tran.Precondition.Trim())));

            //TODO - Throw custom exception if multiple transitions get match
            //TODO - Throw custom exception if no transition get match

            return transitions.First();
        }

        private object GetInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }
            return null;
        }

        private bool RunPrecondition(string precondition)
        {
            IRule rule = (IRule)GetInstance(precondition);
            return rule.Execute();
        }

        public StartCaseOutput StartCase(StartCaseInput caseInput)
        {

            //ToDo: Validations

            StartCaseOutput caseOutput = new StartCaseOutput();

            try
            {
                Transition transition = GetPossibleTransition(new Transition()
                {
                    CurrentPhase = string.Empty,
                    CurrentStatus = string.Empty,
                    Action = caseInput.ACP ? "NewACP" : "NewNonACP"
                });

                UpdateCaseDetailsInDb();

                caseOutput.Success = true;
            }
            catch (Exception ex)
            {
                caseOutput.Success = false;
                caseOutput.ErrorMessage = ex.Message;
            }
            return caseOutput;
        }

        public ClaimCaseOutput ClaimCase(ClaimCaseInput caseInput)
        {
            ClaimCaseOutput caseOutput = new ClaimCaseOutput();

            try
            {
                Transition transition = GetPossibleTransition(new Transition()
                {
                    CurrentPhase = "Intake",
                    CurrentStatus = caseInput.ACP ? "Pending Assignment" : "Unassigned",
                    Action = "Claim"
                });

                UpdateCaseDetailsInDb();

                caseOutput.Success = true;

                return caseOutput;
            }
            catch (Exception ex)
            {
                caseOutput.Success = false;
                caseOutput.ErrorMessage = ex.Message;
            }
            return caseOutput;
        }

        public TransferCaseOutput TransferCase(TransferCaseInput caseInput)
        {
            TransferCaseOutput caseOutput = new TransferCaseOutput();

            //Todo: Validations

            UpdateCaseTransferDetails();

            return caseOutput;
        }

        private void UpdateCaseTransferDetails()
        {
            throw new NotImplementedException();
        }

        private void UpdateCaseDetailsInDb()
        {
            throw new NotImplementedException();
        }
    }

    public class WorkflowInput
    {
        public int Actor { get; set; }
        public string WorkflowType { get; set; }
        public int RecordNumber { get; set; }

    }

    public class WorkflowOutput
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class StartCaseInput : WorkflowInput
    {
        public bool ACP { get; set; }
    }

    public class ClaimCaseInput : WorkflowInput
    {
        public bool ACP { get; set; }

    }

    public class TransferCaseInput : WorkflowInput
    {
        public bool ACP { get; set; }
    }

    public class TransferCaseOutput : WorkflowOutput
    {

    }


    public class StartCaseOutput : WorkflowOutput
    {

    }

    public class ClaimCaseOutput : WorkflowOutput
    {
    }
}
