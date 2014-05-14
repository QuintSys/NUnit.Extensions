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
            _loginViewModel.ShouldHave(expression: x => x.UserName, 
                attributes: typeof (RequiredAttribute));
        }
        
        [Test]
        public void UserName_Should_Have_A_Maximum_Length_Of_256_Characters()
        {
            _loginViewModel.ShouldHave(expression: x => x.UserName, 
                defaultArgumentValue: 256,
                attributes: typeof (StringLengthAttribute));
        }

