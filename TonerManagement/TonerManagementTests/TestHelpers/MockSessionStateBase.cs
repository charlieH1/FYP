using System.Collections.Generic;
using System.Web;

namespace TonerManagementTests.TestHelpers
{
    public class MockSessionStateBase : HttpSessionStateBase
    {
        private readonly Dictionary<string, object> _sessionDictionary = new Dictionary<string, object>();

        public override object this[string name]
        {
            get => _sessionDictionary[name];
            set => _sessionDictionary[name] = value;
        }
    }
}
