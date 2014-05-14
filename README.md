Quintsys.NUnit.Extensions
========================

Extensions for NUnit testing framework.


Installation
------------

To install Quintsys.NUnit.Extensions, run the following command in the Package Manager Console:

    PM> Install-Package Quintsys.NUnit.Extensions


Usage
-----

```

        using NUnit.Framework;
        using Quintsys.NUnit.Extensions;
        
        private readonly LoginViewModel _loginViewModel = new LoginViewModel();

        
        [Test]
        public void UserName_Should_Be_Required()
        {
            _loginViewModel.ShouldHave(expression: x => x.UserName, defaultArgumentValue: false, attributes: typeof (RequiredAttribute));
        }

