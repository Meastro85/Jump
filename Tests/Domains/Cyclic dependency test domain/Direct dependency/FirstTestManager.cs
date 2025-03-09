using Jump.Attributes.Components;

namespace Tests.Domains.Cyclic_dependency_test_domain.Direct_dependency;

[Service]
public class FirstTestManager(SecondTestManager secondTestManager);