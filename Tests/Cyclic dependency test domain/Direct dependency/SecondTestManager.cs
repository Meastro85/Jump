using Jump.Attributes.Components;

namespace Tests.Cyclic_dependency_test_domain.Direct_dependency;

[Service]
public class SecondTestManager(FirstTestManager manager);