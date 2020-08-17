using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkflowModule
{
    public class Workflow
    {

        public Workflow()
        {
            Transition newAcpCase = new Transition()
            {
                CurrentPhase = string.Empty,
                CurrentStatus = string.Empty,
                Action = "NewACP",
                ResultPhase = "Intake",
                ResultStatus = "PendingAssignment",
                Condition = "ACP"
            };
            Transition newNonAcpCase = new Transition()
            {
                CurrentPhase = string.Empty,
                CurrentStatus = string.Empty,
                Action = "NewNonACP",
                ResultPhase = "Intake",
                ResultStatus = "Unassigned",
                Condition = "NonACP"
            };
            Transition claimNonAcpCase = new Transition()
            {
                CurrentPhase = "Intake",
                CurrentStatus = "Assigned",
                Action = "Claim",
                ResultPhase = "RootCause",
                ResultStatus = "Assigned",
                Condition = string.Empty
            };
            Transition claimAcpCase = new Transition()
            {
                CurrentPhase = "Intake",
                CurrentStatus = "Pending Assignment",
                Action = "Claim",
                ResultPhase = "RootCause",
                ResultStatus = "Assigned",
                Condition = string.Empty
            };
        }

        public List<Transition> Transitions { get; }

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

        public List<Transition> GetPossibleTransitions(Transition transition)
        {
            List<Transition> transitions = null;
            transitions = Transitions.FindAll(tran => tran.CurrentStatus == transition.CurrentStatus && tran.CurrentPhase == transition.CurrentPhase);
            if (transitions == null)
            {
                throw new Exception("Need to implement custom workflow exceptions");
            }

            return transitions;
        }

        public StartCaseOutput StartCase(StartCaseInput caseInput)
        {

            //ToDo: Validations

            StartCaseOutput caseOutput = new StartCaseOutput();

            try
            {
                List<Transition> transitions = GetPossibleTransitions(new Transition()
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
                List<Transition> transitions = GetPossibleTransitions(new Transition()
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
