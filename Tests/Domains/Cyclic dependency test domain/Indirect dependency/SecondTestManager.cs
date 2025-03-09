using Jump.Attributes.Components;

namespace Tests.Domains.Cyclic_dependency_test_domain.Indirect_dependency;

[Service]
public class SecondTestManager(ThirdTestManager manager);