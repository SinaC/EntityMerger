﻿using System;

namespace EntityMerger.UnitTest.Entities.Simple;

internal class EntityLevel2 : PersistEntity
{
    public Guid Id { get; set; }

    public string DeliveryPointEan { get; set; } = null!;

    public decimal Value1 { get; set; }
    public decimal? Value2 { get; set; }

    // debug property, will never participate in merge neither as key nor value
    public int Index { get; set; }
}
