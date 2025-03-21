﻿using Jump.Attributes;
using Jump.Attributes.Components;
using Tests.Component_test_domain.Hop_test;
using Tests.Component_test_domain.Managers;

namespace Tests.Component_test_domain.Configurations;

[Configuration]
public class HopConfiguration
{
    [Hop]
    public static HopController HopController(TestManager manager)
    {
        return new HopController(manager, "This is a hop test");
    }
}