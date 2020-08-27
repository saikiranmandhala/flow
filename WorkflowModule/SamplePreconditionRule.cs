using System;
using System.Collections.Generic;
using System.Text;
using WorkflowModule;

namespace WorkflowModule //SamplePreconditionNamespace
{
    public class SamplePreconditionRule : IRule
    {
        public bool Execute()
        {
            Random random = new Random();
            bool value = random.Next(1, 20) % 2 == 0 ? true : false;
            return value;
        }
    }

    public class AccountsTeamconditionRule : IRule
    {
        public bool Execute()
        {
            Random random = new Random();
            bool value = random.Next(1, 20) % 2 == 0 ? true : false;
            return value;
        }
    }
}
