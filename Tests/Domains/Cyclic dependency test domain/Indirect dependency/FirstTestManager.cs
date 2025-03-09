using Jump.Attributes.Components;

namespace Tests.Cyclic_dependency_test_domain.Indirect_dependency;

[Service]
public class FirstTestManager(SecondTestManager manager);